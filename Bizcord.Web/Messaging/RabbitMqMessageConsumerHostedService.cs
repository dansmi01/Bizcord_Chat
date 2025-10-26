using Bizcord.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Bizcord.Web.Messaging
{
    public class RabbitMqMessageConsumerHostedService : BackgroundService
    {
        private readonly RabbitMqConnection _conn;
        private readonly IConfiguration _config;
        private readonly IHubContext<ChatHub> _hub;
        private readonly ILogger<RabbitMqMessageConsumerHostedService> _logger;

        public RabbitMqMessageConsumerHostedService(
            RabbitMqConnection conn,
            IConfiguration config,
            IHubContext<ChatHub> hub,
            ILogger<RabbitMqMessageConsumerHostedService> logger)
        {
            _conn = conn;
            _config = config;
            _hub = hub;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var exchange = _config["RabbitMQ:Exchange"];   // e.g. bizcord.exchange
            var queue = _config["RabbitMQ:Queue"];         // e.g. bizcord.messages
            var routingKey = _config["RabbitMQ:RoutingKey"]; // e.g. message.created

            var connection = _conn.GetConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange, ExchangeType.Direct, durable: true, autoDelete: false);
            channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(queue, exchange, routingKey);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var evt = JsonSerializer.Deserialize<MessageEvent>(json);
                    if (evt != null)
                    {
                        // Broadcast to all connected clients
                        await _hub.Clients.All.SendAsync("ReceiveMessage", evt, cancellationToken: stoppingToken);
                    }
                    channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, "Error consuming message");
                    channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

            channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }
    }

    public record MessageEvent
    {
        public long Id { get; init; }
        public int ChannelId { get; init; }
        public string Content { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public System.DateTime CreatedAt { get; init; }
    }
}