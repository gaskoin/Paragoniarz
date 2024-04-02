using System;
using System.Collections.Generic;

namespace Paragoniarz.Domain.Orders;

public partial class Order
{
    private readonly List<Product> products = [];

    public string Id { get; private set; }
    public DateTime? Date { get; private set; }
    public decimal? TotalPrice { get; private set; }
    public Buyer? Buyer { get; private set; }
    public string? PaymentType { get; private set; }
    public string? Shipment { get; private set; }
    public decimal? ShipmentPrice { get; private set; }

    public IEnumerable<Product> Products { get { return new List<Product>(products); } }

    private Order()
    {
    }

    public bool IsValid()
    {
        return Date is not null
            && TotalPrice is not null
            && ShipmentPrice is not null
            && Buyer is not null
            && !string.IsNullOrEmpty(PaymentType)
            && !string.IsNullOrEmpty(Shipment);
    }

    public void AddProduct(Product product)
    {
        products.Add(product);
    }

    public override bool Equals(object? obj)
    {
        return obj is Order order && Id == order.Id;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}
