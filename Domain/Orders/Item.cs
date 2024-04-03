using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Paragoniarz.Domain.Orders;

public class Item
{
    [XmlElement("ord_date")] public string? RawDate { get; set; }
    [XmlElement("total_price")] public string? RawTotalPrice { get; set; }
    [XmlElement("ord_ship_price")] public string? RawShipmentPrice { get; set; }
    [XmlElement("op_amount")] public string? RawQuantity { get; set; }
    [XmlElement("op_price_net")] public string? RawNetPrice { get; set; }
    [XmlElement("op_prod_tax")] public string? RawTax { get; set; }
    [XmlElement("op_price")] public string? RawUnitPrice { get; set; }

    [XmlElement("ord_id")] public string? Id { get; set; }
    [XmlElement("ord_firstname")] public string? FirstName { get; set; }
    [XmlElement("ord_lastname")] public string? LastName { get; set; }
    [XmlElement("ord_email")] public string? Email { get; set; }
    [XmlElement("ord_street")] public string? Street { get; set; }
    [XmlElement("ord_code")] public string? ZipCode { get; set; }
    [XmlElement("ord_city")] public string? City { get; set; }
    [XmlElement("payment")] public string? PaymentType { get; set; }
    [XmlElement("shipment")] public string? Shipment { get; set; }
    [XmlElement("prod_name")] public string? ProductName { get; set; }

    [XmlIgnore] public decimal? TotalPrice => GetNullDecimal(RawTotalPrice);
    [XmlIgnore] public decimal? ShipmentPrice => GetNullDecimal(RawShipmentPrice);
    [XmlIgnore] public int? Quantity => GetNullInt(RawQuantity);
    [XmlIgnore] public decimal? NetPrice => GetNullDecimal(RawNetPrice);
    [XmlIgnore] public decimal? Tax => GetNullDecimal(RawTax);
    [XmlIgnore] public decimal? UnitPrice => GetNullDecimal(RawUnitPrice);

    [XmlIgnore]
    public DateTime? Date
    {
        get
        {
            if (string.IsNullOrEmpty(RawDate))
                return null;

            return DateTime.Parse(RawDate, CultureInfo.InvariantCulture);
        }
    }

    public Order AsOrder()
    {
        var address = new Address()
        {
            Street = Street!,
            ZipCode = ZipCode!,
            City = City!
        };

        var buyer = new Buyer()
        {
            FirstName = FirstName!,
            LastName = LastName!,
            Email = Email!,
            InvoiceAddress = address
        };

        return new Order()
        {
            Id = Id!,
            Date = Date!.Value,
            TotalPrice = TotalPrice!.Value,
            Buyer = buyer,
            PaymentType = PaymentType!,
            Shipment = Shipment!,
            ShipmentPrice = ShipmentPrice!.Value
        };
    }

    public Product AsProduct()
    {
        return new Product()
        {
            ProductName = ProductName!,
            Quantity = Quantity!.Value,
            NetPrice = NetPrice!.Value,
            Tax = Tax!.Value,
            UnitPrice = UnitPrice!.Value,
        };
    }

    private decimal? GetNullDecimal(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        return decimal.Parse(value, CultureInfo.InvariantCulture);
    }

    private int? GetNullInt(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        return int.Parse(value, CultureInfo.InvariantCulture);
    }

    public bool IsOrder()
    {
        bool orderFieldsAreSet = !string.IsNullOrEmpty(Id)
                              && !string.IsNullOrEmpty(RawTotalPrice)
                              && !string.IsNullOrEmpty(RawShipmentPrice)
                              && !string.IsNullOrEmpty(RawDate)
                              && !string.IsNullOrEmpty(FirstName)
                              && !string.IsNullOrEmpty(LastName)
                              && !string.IsNullOrEmpty(Email)
                              && !string.IsNullOrEmpty(Street)
                              && !string.IsNullOrEmpty(ZipCode)
                              && !string.IsNullOrEmpty(City)
                              && !string.IsNullOrEmpty(PaymentType)
                              && !string.IsNullOrEmpty(Shipment);

        bool productFieldsAreSet = !string.IsNullOrEmpty(Id)
                                && !string.IsNullOrEmpty(RawQuantity)
                                && !string.IsNullOrEmpty(RawNetPrice)
                                && !string.IsNullOrEmpty(RawTax)
                                && !string.IsNullOrEmpty(RawUnitPrice)
                                && !string.IsNullOrEmpty(ProductName);

        return orderFieldsAreSet && productFieldsAreSet;
    }

    public static IEnumerable<string> GetRequiredXmlFields()
    {
        return typeof(Item).GetProperties()
                           .Select(property => property.GetCustomAttribute<XmlElementAttribute>(false))
                           .Where(attribute => attribute is not null)
                           .Select(attribute => attribute.ElementName);
    }
}