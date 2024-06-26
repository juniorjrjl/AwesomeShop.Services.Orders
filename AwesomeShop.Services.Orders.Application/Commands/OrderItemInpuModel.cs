namespace AwesomeShop.Services.Orders.Application.Commands;

public record OrderItemInpuModel(Guid ProductId, int Amount, decimal Price);
