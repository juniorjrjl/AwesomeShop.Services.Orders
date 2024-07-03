using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;

namespace AwesomeShop.Services.Orders.Infrastructure.MessageBus;

public class RabbitMQClient(ProducerConnection producerConnection) : IMessageBusClient
{

    private readonly IConnection _connection = producerConnection.Connection;

    public void Publish(object message, string routingKey, string exchange)
    {
        var channel = _connection.CreateModel();

        var settins = new JsonSerializerSettings{
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        var payload = JsonConvert.SerializeObject(message, settins);
        var body = Encoding.UTF8.GetBytes(payload);

        channel.ExchangeDeclare(exchange, ExchangeType.Topic, true);
        channel.BasicPublish(exchange, routingKey, null, body);
    }
}