using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using log4net;

namespace Paragoniarz.Domain.Orders;
public class OrderSummaryService : IOrderSummaryService
{
    private static readonly ILog log = LogManager.GetLogger(typeof(OrderSummaryService));
    private static readonly string Preamble = "<orders xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"";

    public async Task<OrderSummary> ReadFile(Uri path)
    {
        log.Info($"Reading file {path.LocalPath}");
        if (await path.IsBinaryFileAsync())
            throw new Exception("Cannot process binary file");

        string contents = await File.ReadAllTextAsync(path.LocalPath);
        return ToOrderSummary(contents);
    }

    private OrderSummary ToOrderSummary(string xmlOrders)
    {
        log.Info("Deserializing orders");
        if (!Validate(xmlOrders))
            throw new Exception("Validation exception");

        XDocument document = XDocument.Parse(xmlOrders);

        return new OrderSummary(
            ExtractDate(document),
            ExtractElements(document).Select(item => new Item(MapToOrder(item), MapToProduct(item)))
                                     .Aggregate(new Dictionary<string, Order>(), AccumulateOrders, map => map.Values)
        );
    }

    private bool Validate(string contents) => contents.Contains(Preamble);

    private Order MapToOrder(XElement item)
    {
        Address address = MapToAdrress(item);
        Buyer buyer = MapToBuyer(item, address);

        return new Order.OrderBuilder()
            .WithId(item.GetString("ord_id")!)
            .WithDate(item.GetDateTime("ord_date"))
            .WithTotalPrice(item.GetDecimal("total_price"))
            .WithShipment(item.GetString("shipment"))
            .WithShipmentPrice(item.GetDecimal("ord_ship_price"))
            .WithPaymentType(item.GetString("payment"))
            .WithBuyer(buyer)
            .Build();
    }

    private Address MapToAdrress(XElement item)
    {
        return new(
            item.GetString("ord_street"),
            item.GetString("ord_code"),
            item.GetString("ord_city")
        );
    }

    private Buyer MapToBuyer(XElement item, Address adrress)
    {
        return new Buyer.BuyerBuilder()
            .WithFirstName(item.GetString("ord_firstname"))
            .WithLastName(item.GetString("ord_lastname"))
            .WithEmail(item.GetString("ord_email"))
            .WithAddress(adrress)
            .Build();
    }

    private Product MapToProduct(XElement item)
    {
        return new Product(
            item.GetString("prod_name"),
            item.GetInt("op_amount"),
            item.GetDecimal("op_price_net"),
            item.GetDecimal("op_prod_tax"),
            item.GetDecimal("op_price")
        );
    }

    private Dictionary<string, Order> AccumulateOrders(Dictionary<string, Order> orders, Item item)
    {
        Order order = item.Order;
        if (order.IsValid())
            orders.Add(order.Id, order);

        orders[order.Id].AddProduct(item.Product);
        return orders;
    }

    private DateTime ExtractDate(XDocument document)
    {
        return document.Element("orders")!.GetDateTimeAttribute("date");
    }

    private IEnumerable<XElement> ExtractElements(XDocument document)
    {
        return document.Element("orders")!.Elements("item");
    }
}