using System;
using System.Collections.Generic;

namespace Paragoniarz.Domain.Orders;

public class Order
{
    private readonly List<Product> products = [];

    public required string Id { get; set; }
    public required DateTime Date { get; set; }
    public required decimal TotalPrice { get; set; }
    public required Buyer Buyer { get; set; }
    public required string PaymentType { get; set; }
    public required string Shipment { get; set; }
    public required decimal ShipmentPrice { get; set; }

    public IEnumerable<Product> Products { get { return new List<Product>(products); } }

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