using System;
using System.IO;
using System.Threading.Tasks;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using log4net;
using Paragoniarz.Domain.Orders;
using Paragoniarz.Domain.Settings;

namespace Paragoniarz.Domain;
public class DocumentService(IConfigurationService configurationService) : IDocumentService
{
    private static readonly ILog log = LogManager.GetLogger(typeof(DocumentService));
    private readonly Configuration configuration = configurationService.LoadConfiguration();

    public async Task<OrderSummary?> GenerateDocuments(OrderSummary summary, IProgress<int> progress)
    {
        string assets = configuration.AssetsDirectory;
        string documentsDirectory = configuration.DocumentsDirectory;

        string template = await File.ReadAllTextAsync($"{assets}/template.html");
        string rowTemplate = await File.ReadAllTextAsync($"{assets}/row_template.html");

        log.Info("Generating documents");
        foreach (Order order in summary.Orders)
        {
            string rows = string.Empty;
            foreach (Product product in order.Products)
                rows += FillTemplate(rowTemplate, product);

            string contents = FillTemplate(template, order)
                .Replace("${confirmationDate}", summary.Date.ToString("dd'/'MM'/'yyyy"))
                .Replace("${rows}", rows);

            var fileName = $"{documentsDirectory}/potwierdzenie_{order.Id}.pdf";
            log.Info($"Saving {fileName}");
            await using FileStream stream = File.OpenWrite(fileName);
            HtmlConverter.ConvertToPdf(contents, stream, GetConverterProperties(assets));

            progress.Report(1);
        }

        return summary;
    }

    private string FillTemplate(string template, Product product)
    {
        return template.Replace("${productName}", product.ProductName)
                       .Replace("${quantity}", product.Quantity.ToString())
                       .Replace("${netPrice}", product.NetPrice.ToString())
                       .Replace("${tax}", product.Tax.ToString())
                       .Replace("${unitPrice}", product.UnitPrice.ToString())
                       .Replace("${totalPrice}", product.TotalPrice.ToString());
    }

    private string FillTemplate(string template, Order order)
    {
        SellerConfiguration configuration = configurationService.LoadConfiguration().SellerConfiguration;

        return template.Replace("${orderId}", order.Id)
                       .Replace("${orderDate}", order.Date.ToString("dd'/'MM'/'yyyy"))
                       .Replace("${firstName}", order.Buyer.FirstName)
                       .Replace("${lastName}", order.Buyer.LastName)
                       .Replace("${street}", order.Buyer.InvoiceAddress.Street)
                       .Replace("${postcode}", order.Buyer.InvoiceAddress.ZipCode)
                       .Replace("${city}", order.Buyer.InvoiceAddress.City)
                       .Replace("${shipmentPrice}", order.ShipmentPrice.ToString())
                       .Replace("${totalPrice}", order.TotalPrice.ToString())
                       .Replace("${shipment}", order.Shipment)
                       .Replace("${paymentType}", order.PaymentType)
                       .Replace("${seller.city}", configuration.City)
                       .Replace("${seller.name}", configuration.Name)
                       .Replace("${seller.address}", configuration.Address)
                       .Replace("${seller.zipCode}", configuration.ZipCode)
                       .Replace("${seller.nip}", configuration.NIP);
    }

    private ConverterProperties GetConverterProperties(string resourceDirectory)
    {
        ConverterProperties properties = new();
        properties.SetBaseUri(resourceDirectory);
        properties.SetFontProvider(new DefaultFontProvider(true, true, true));
        return properties;
    }
}
