using AwesomeShop.Services.Orders.Core.ValueObjects;

namespace AwesomeShop.Services.Orders.Core.Events;

public record OrderCreated(Guid Id, decimal Total, PaymentInfo PaymentInfo, string FullName, string Email) : IDomainEvent
{

}