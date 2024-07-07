using System.Text;
using AwesomeShop.Services.Orders.Core.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.CacheStorage;
using AwesomeShop.Services.Orders.Infrastructure.MessageBus;
using AwesomeShop.Services.Orders.Infrastructure.Persistence;
using AwesomeShop.Services.Orders.Infrastructure.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.ServiceDiscovery;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using RabbitMQ.Client;

namespace AwesomeShop.Services.Orders.Infrastructure;

public static class Extensions
{
    
    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        #pragma warning disable CS0618
        BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
        BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
        #pragma warning restore CS0618

        services.AddSingleton(s => {
            var configuration = s.GetService<IConfiguration>();
            ArgumentNullException.ThrowIfNull(configuration);
            var mongoConfig = configuration.GetSection("Mongo").Get<MongoDBOptions>();
            ArgumentNullException.ThrowIfNull(mongoConfig);

            return mongoConfig;
        });

        services.AddSingleton<IMongoClient>(s => {
            var options = s.GetService<MongoDBOptions>();
            ArgumentNullException.ThrowIfNull(options);

            return new MongoClient(options.ConnectionStrings);
        });

        services.AddTransient(s => {

            var options = s.GetService<MongoDBOptions>();
            ArgumentNullException.ThrowIfNull(options);
            var mongoClient =  s.GetService<IMongoClient>();
            ArgumentNullException.ThrowIfNull(mongoClient);

            return mongoClient.GetDatabase(options.Database);
        });
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }

    public static IServiceCollection AddRabbitMq(this IServiceCollection services) {
        services.AddSingleton(s => {
            var configuration = s.GetService<IConfiguration>();
            ArgumentNullException.ThrowIfNull(configuration);

            var rabbitMQConfig = configuration.GetSection("RabbitMQ").Get<RabbitMQOptions>();
            ArgumentNullException.ThrowIfNull(rabbitMQConfig);

            return rabbitMQConfig;
        });
        services.AddSingleton(s => {
            var options = s.GetService<RabbitMQOptions>();
            ArgumentNullException.ThrowIfNull(options);

            var connectionFactory = new ConnectionFactory {
                HostName = options.Host,
                UserName = options.User,
                Password = options.Password,
                Port = options.Port,
                VirtualHost = options.VirtualHost
            };

            var connection = connectionFactory.CreateConnection("order-service-producer"); 

            return new ProducerConnection(connection);
        });  
        services.AddSingleton<IMessageBusClient, RabbitMQClient>();
        
        return services;    
    }

    public static string ToDashCase(this string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        
        if (text.Length < 2) {
            return text;
        }
        var sb = new StringBuilder();
        sb.Append(char.ToLowerInvariant(text[0]));
        for(int i = 1; i < text.Length; ++i) {
            char c = text[i];
            if(char.IsUpper(c)) {
                sb.Append('-');
                sb.Append(char.ToLowerInvariant(c));
            } else {
                sb.Append(c);
            }
        }

        Console.WriteLine($"ToDashCase: "+ sb.ToString());

        return sb.ToString();
    }

    public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient( consulConfig =>
        {
            var address = config.GetValue<string>("Consul:Host");
            ArgumentNullException.ThrowIfNull(address);
            consulConfig.Address = new Uri(address);
        }
        ));
        services.AddTransient<IServiceDiscoveryService, ConsulService>();
        return services;
    }

    public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
    {
        var consultClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
        var lifeTime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
        var registration = new AgentServiceRegistration
        {
            ID = $"order-service{Guid.NewGuid()}",
            Name = "OrderServices",
            Address = "orderapi",
            Port = 5001
        };
        consultClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
        consultClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

        Console.WriteLine("Service registered in Consul");

        lifeTime.ApplicationStopping.Register(() =>
        {
            consultClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            Console.WriteLine("Service deregistered in Consul");
        });

        return app;
    }

    public static IServiceCollection AddRedisCache(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(opt =>
        {
            opt.InstanceName = "OrdersCache";
            opt.Configuration = "dbcache:6379";
        });
        services.AddTransient<ICacheService, RedisCacheService>();
        return services;
    }

}
