namespace AwesomeShop.Services.Orders.Application.Commands;

public record PaymentInfoInputModel(string CardNumber, string FullName, string Expiration, string Cvv);
