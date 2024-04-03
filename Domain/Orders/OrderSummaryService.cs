using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using log4net;

namespace Paragoniarz.Domain.Orders;
public class OrderSummaryService : IOrderSummaryService
{
    private static readonly ILog log = LogManager.GetLogger(typeof(OrderSummaryService));

    public async Task<OrderSummary> ReadFile(Uri path)
    {
        log.Info($"Reading file {path.LocalPath}");
        if (await path.IsBinaryFileAsync())
            throw new Exception("Cannot process binary file");

        var xml = await File.ReadAllTextAsync(path.LocalPath);
        return ToOrderSummary(xml);
    }

    private OrderSummary ToOrderSummary(string xml)
    {
        Validate(xml);

        using (var stream = new StringReader(xml))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Orders));
            log.Info("Deserializing orders");
            Orders? orders = serializer.Deserialize(stream) as Orders;

            return new OrderSummary(
                orders!.Date!.Value,
                orders.Items.Aggregate(new Dictionary<string, Order>(), AccumulateOrders, map => map.Values)
            );
        }
    }

    private void Validate(string xml)
    {
        List<string> missingFields = [];
        using (var stream = new StringReader(xml))
        {
            var document = XDocument.Load(stream);

            var requiredElements = Item.GetRequiredXmlFields();
            foreach (var requiredElement in requiredElements)
            {
                bool containsElement = document.Descendants(requiredElement).Any();
                if (!containsElement)
                    missingFields.Add(requiredElement);
            }
        }

        if (missingFields.Count > 0)
            throw new ValidationException(missingFields);
    }

    private Dictionary<string, Order> AccumulateOrders(Dictionary<string, Order> orders, Item item)
    {
        if (item.IsOrder())
        {
            Order order = item.AsOrder();
            orders.Add(order.Id, order);
        }

        orders[item.Id!].AddProduct(item.AsProduct());
        return orders;
    }
}