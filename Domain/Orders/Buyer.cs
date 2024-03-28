namespace Paragoniarz.Domain.Orders;

public class Buyer
{
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Email { get; private set; }
    public Address? InvoiceAddress { get; private set; }

    private Buyer() { }

    public class BuyerBuilder()
    {
        private readonly Buyer buyer = new();

        public BuyerBuilder WithFirstName(string? firstName)
        {
            buyer.FirstName = firstName;
            return this;
        }

        public BuyerBuilder WithLastName(string? lastName)
        {
            buyer.LastName = lastName;
            return this;
        }

        public BuyerBuilder WithEmail(string? email)
        {
            buyer.Email = email;
            return this;
        }

        public BuyerBuilder WithAddress(Address? address)
        {
            buyer.InvoiceAddress = address;
            return this;
        }

        public Buyer Build()
        {
            return buyer;
        }
    }
}