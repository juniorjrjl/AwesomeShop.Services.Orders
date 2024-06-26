namespace AwesomeShop.Services.Orders.Core.Entities;

public class OrderItem(Guid productId, int amount, decimal price) : IEntityBase
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid ProductId { get; private set; } = productId;

    public int Amount { get; private set; } = amount;

    public decimal Price { get; private set; } = price;
    

}