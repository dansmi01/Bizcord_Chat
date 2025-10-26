using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bizcord.Web.Messaging;

public class RabbitMqMessagePublisher : IMessagePublisher
{
    private readonly RabbitMqConnection _conn;
    private readonly IConfiguration _config;
    public RabbitMqMessagePublisher(RabbitMqConnection conn, IConfiguration config)
    { _conn = conn; _config = config; }


    public Task PublishMessageCreatedAsync(MessageEvent evt)
    {
        var exchange = _config["RabbitMQ:Exchange"]; // fanout or direct (using routing key)
        var routingKey = _config["RabbitMQ:RoutingKey"]; // e.g., message.created


        using var connection = _conn.GetConnection();
        using var channel = connection.CreateModel();


        channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct, durable: true, autoDelete: false);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt));
        var props = channel.CreateBasicProperties();
        props.ContentType = "application/json";
        props.DeliveryMode = 2; // persistent


        channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: props, body: body);
        return Task.CompletedTask;
    }
}