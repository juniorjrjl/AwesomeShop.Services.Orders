using AwesomeShop.Services.Orders.Application.Commands;
using AwesomeShop.Services.Orders.Core.Entities;
using AwesomeShop.Services.Orders.Core.ValueObjects;

namespace AwesomeShop.Services.Orders.Application.Mappers;

public class OrderMapper : IOrderMapper
{
    public Order ToOrder(AddOrder request)
    {
        return new Order(
            customer: ToCustomer(request.Customer), 
            deliveryAddress: ToDeliveryAddress(request.DeliveryAddress), 
            paymentAddress: ToPaymentAddress(request.PaymentAddress), 
            paymentInfo: ToPaymentInfo(request.PaymentInfo),
            items: request.Items.Select(ToOrderItem).ToList()
        );
    }

    private static Customer ToCustomer(CustomerInputModel inputModel) => 
        new(id: inputModel.Id, fullName: inputModel.FullName, email: inputModel.Email);

    private static DeliveryAddress ToDeliveryAddress(DeliveryAddressInputModel inputModel) => 
        new(Street: inputModel.Street, Number: inputModel.Number, City: inputModel.City, State: inputModel.State, ZipCode: inputModel.ZipCode);

    private static PaymentAddress ToPaymentAddress(PaymentAddressInputModel inputModel) =>
        new(Street: inputModel.Street, Number: inputModel.Number, City: inputModel.City, State: inputModel.State, ZipCode: inputModel.ZipCode);

    private static PaymentInfo ToPaymentInfo(PaymentInfoInputModel inputModel) =>
        new(CardNumber: inputModel.CardNumber, FullName: inputModel.FullName, Expiration: inputModel.Expiration, Cvv: inputModel.Cvv);

    private static OrderItem ToOrderItem(OrderItemInpuModel inputModel) =>
        new(productId: inputModel.ProductId, amount: inputModel.Amount, price: inputModel.Price);
}