namespace Paragoniarz.Domain.Orders;

public record Product(string? ProductName, int? Quantity, decimal? NetPrice, decimal? Tax, decimal? UnitPrice)
{
    public decimal? TotalPrice
    {
        get { return Quantity * UnitPrice; }
    }
}
