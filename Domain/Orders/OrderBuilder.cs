using System;

namespace Paragoniarz.Domain.Orders;

public partial class Order
{
    public class OrderBuilder
    {
        private readonly Order order = new();

        public OrderBuilder WithId(string id)
        {
            order.Id = id;
            return this;
        }

        public OrderBuilder WithDate(DateTime? date)
        {
            order.Date = date;
            return this;
        }

        public OrderBuilder WithTotalPrice(decimal? price)
        {
            order.TotalPrice = price;
            return this;
        }

        public OrderBuilder WithBuyer(Buyer? buyer)
        {
            order.Buyer = buyer;
            return this;
        }

        public OrderBuilder WithPaymentType(string? paymentType)
        {
            order.PaymentType = paymentType;
            return this;
        }

        public OrderBuilder WithShipment(string? shipment)
        {
            order.Shipment = shipment;
            return this;
        }

        public OrderBuilder WithShipmentPrice(decimal? shipmentPrice)
        {
            order.ShipmentPrice = shipmentPrice;
            return this;
        }

        public Order Build()
        {
            return order;
        }
    }
}