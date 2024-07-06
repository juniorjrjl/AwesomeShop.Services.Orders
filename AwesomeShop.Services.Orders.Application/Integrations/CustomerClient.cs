using AwesomeShop.Services.Orders.Application.DTOs.Integration;
using AwesomeShop.Services.Orders.Infrastructure.ServiceDiscovery;
using Newtonsoft.Json;

namespace AwesomeShop.Services.Orders.Application.Integrations;

public class CustomerClient(IServiceDiscoveryService serviceDiscovery, HttpClient httpClient) : ICustomerClient
{
    private readonly IServiceDiscoveryService _serviceDiscovery = serviceDiscovery;
    private readonly HttpClient _httpClient = httpClient;

    public async Task<GetCustomerByIdDTO> GetByIdAsync(Guid Id)
    {
        
        var customerUrl = await _serviceDiscovery.GetServiceUri("CustomerServices", $"api/Customers/{Id}");
        var result = await _httpClient.GetAsync(customerUrl);
        var stringResult = await result.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<GetCustomerByIdDTO>(stringResult);
        ArgumentNullException.ThrowIfNull(response);
        return response;
    }

}
