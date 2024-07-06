using AwesomeShop.Services.Orders.Application.Commands;
using AwesomeShop.Services.Orders.Application.Integrations;
using AwesomeShop.Services.Orders.Application.Mappers;
using AwesomeShop.Services.Orders.Application.Subscribers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeShop.Services.Orders.Application;

public static class Extensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AddOrder).Assembly));

        return services;
    }

    public static IServiceCollection AddSubscribers(this IServiceCollection services)
    {
        services.AddHostedService<PaymentAcceptedSubscriber>();
        return services;
    }

    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddSingleton<IOrderMapper, OrderMapper>();
        return services;
    }

    public static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddSingleton<ICustomerClient, CustomerClient>();
        return services;
    }

}
