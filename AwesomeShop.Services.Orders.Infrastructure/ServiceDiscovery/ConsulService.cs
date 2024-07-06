
using System.Linq;
using Consul;

namespace AwesomeShop.Services.Orders.Infrastructure.ServiceDiscovery;

public class ConsulService(IConsulClient consulClient) : IServiceDiscoveryService
{

    private readonly IConsulClient _consulClient = consulClient;

    public async Task<Uri> GetServiceUri(string serviceName, string requestUrl)
    {
        var allRegisteredService = await _consulClient.Agent.Services();
        var registeredServices = allRegisteredService.Response?
            .Where(s => s.Value.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase))
            .Select(s => s.Value)
            .ToList();
        ArgumentNullException.ThrowIfNull(registeredServices);
        
        var service = registeredServices.First();
        Console.WriteLine(service.Address);
        var uri = $"http://{service.Address}:{service.Port}/{requestUrl}";
        return new Uri(uri);
    }
}