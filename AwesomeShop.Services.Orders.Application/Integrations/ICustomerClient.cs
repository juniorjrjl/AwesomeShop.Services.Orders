using AwesomeShop.Services.Orders.Application.DTOs.Integration;

namespace AwesomeShop.Services.Orders.Application.Integrations;

public interface ICustomerClient
{
    Task<GetCustomerByIdDTO> GetByIdAsync(Guid Id);
}
