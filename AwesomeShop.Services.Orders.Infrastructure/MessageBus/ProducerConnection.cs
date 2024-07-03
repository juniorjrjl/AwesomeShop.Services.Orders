using RabbitMQ.Client;

namespace AwesomeShop.Services.Orders.Infrastructure.MessageBus;

public record ProducerConnection(IConnection Connection);
