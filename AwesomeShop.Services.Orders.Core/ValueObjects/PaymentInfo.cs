namespace AwesomeShop.Services.Orders.Core.ValueObjects;

public record PaymentInfo(string CardNumber, string FullName, string Expiration, string Cvv)
{

}
