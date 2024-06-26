namespace AwesomeShop.Services.Orders.Application.Commands;

public record PaymentAddressInputModel(string Street, string Number, string City, string State, string ZipCode);
