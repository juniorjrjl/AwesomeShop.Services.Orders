using AwesomeShop.Services.Orders.Core.Enums;
using AwesomeShop.Services.Orders.Core.Events;
using AwesomeShop.Services.Orders.Core.ValueObjects;

namespace AwesomeShop.Services.Orders.Core.Entities;

public class Order : AggregateRoot
{
    public Order(Customer customer, DeliveryAddress deliveryAddress, PaymentAddress paymentAddress, PaymentInfo paymentInfo, List<OrderItem> items)
    {
        Id = Guid.NewGuid();
        Total = items.Sum(i => i.Amount * i.Price);
        Customer = customer;
        DeliveryAddress = deliveryAddress;
        PaymentAddress = paymentAddress;
        PaymentInfo = paymentInfo;
        Items = items;
        Status = OrderStatus.STARTED;
        CreatedAt = DateTime.UtcNow;
        AddEvent(new OrderCreated(Id, Total, paymentInfo, Customer.FullName, Customer.Email));
    }

    public decimal Total { get; private set; }
    
    public Customer Customer { get; private set; }
    
    public DeliveryAddress DeliveryAddress { get; private set; }
    
    public PaymentAddress PaymentAddress { get; private set; }
    
    public PaymentInfo PaymentInfo { get; private set; }

    public List<OrderItem> Items { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public OrderStatus Status { get; private set; }

}