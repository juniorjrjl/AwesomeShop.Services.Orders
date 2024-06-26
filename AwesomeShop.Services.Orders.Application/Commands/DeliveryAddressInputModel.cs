namespace AwesomeShop.Services.Orders.Application.Commands;

public record DeliveryAddressInputModel(string Street, string Number, string City, string State, string ZipCode);
