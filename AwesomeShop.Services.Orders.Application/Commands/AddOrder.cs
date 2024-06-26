using MediatR;

namespace AwesomeShop.Services.Orders.Application.Commands;

public record AddOrder(
    CustomerInputModel Customer, 
    List<OrderItemInpuModel> Items, 
    DeliveryAddressInputModel DeliveryAddress, 
    PaymentAddressInputModel PaymentAddress,
    PaymentInfoInputModel PaymentInfo
) : IRequest<Guid>;
