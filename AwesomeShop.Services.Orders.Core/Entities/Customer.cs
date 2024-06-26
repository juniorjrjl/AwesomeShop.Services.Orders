
namespace AwesomeShop.Services.Orders.Core.Entities;

public class Customer(Guid id, string fullName, string email) : IEntityBase
{

    public Guid Id { get; private set; } = id;
    public string FullName { get; private set; } = fullName;
    public string Email { get; private set; } = email;

}