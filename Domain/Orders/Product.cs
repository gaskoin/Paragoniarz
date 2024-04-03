namespace Paragoniarz.Domain.Orders;

public class Product
{
    public required string ProductName { get; set; }
    public required int Quantity { get; set; }
    public required decimal NetPrice { get; set; }
    public required decimal Tax { get; set; }
    public required decimal UnitPrice { get; set; }

    public decimal? TotalPrice
    {
        get { return Quantity * UnitPrice; }
    }
}
