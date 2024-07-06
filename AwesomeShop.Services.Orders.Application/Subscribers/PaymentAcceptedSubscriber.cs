using System.Text;
using AwesomeShop.Services.Orders.Application.DTOs;
using AwesomeShop.Services.Orders.Core.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.MessageBus;
using AwesomeShop.Services.Orders.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AwesomeShop.Services.Orders.Application.Subscribers;

public class PaymentAcceptedSubscriber : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public PaymentAcceptedSubscriber(IServiceProvider serviceProvider, RabbitMQOptions options)
    {
        _serviceProvider = serviceProvider;
        var connectionFactory = new ConnectionFactory {
            HostName = options.Host,
            UserName = options.User,
            Password = options.Password,
            Port = options.Port,
            VirtualHost = options.VirtualHost
        };
        _connection = connectionFactory.CreateConnection("order-service-payment-accepted-subscriber");
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(Exchange, ExchangeType.Topic, true);
        _channel.QueueDeclare(Queue, false, false, false, null);
        _channel.QueueBind(Queue, "payment-service", RoutingKey);
    }

    private const string Queue = "order-service/payment-accepted";
    private const string Exchange = "order-service";
    private const string RoutingKey = "payment-accepted";

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (sender, args) =>
        {
            var byteArray = args.Body.ToArray();
            var contentString = Encoding.UTF8.GetString(byteArray);
            var message = JsonConvert.DeserializeObject<PaymentAccepted>(contentString); 
            Console.WriteLine($"Message PaymentAccepted received {message}");
            ArgumentNullException.ThrowIfNull(message);
            await UpdateOrder(message);
            _channel.BasicAck(args.DeliveryTag, false);
        };
        _channel.BasicConsume(Queue, false, consumer);
        return Task.CompletedTask;
    }

    private async Task<bool> UpdateOrder(PaymentAccepted paymentAccepted)
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetService<IOrderRepository>();
        ArgumentNullException.ThrowIfNull(repository);
        var order = await repository.GetByIdAsync(paymentAccepted.Id);
        order.SetAsCompleted();
        await repository.UpdateAsync(order);
        return true;
    }

}