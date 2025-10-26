using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;

namespace Bizcord.Web.Messaging;

public class RabbitMqConnection
{
    private readonly IConfiguration _config;
    private IConnection? _connection;
    public RabbitMqConnection(IConfiguration config) => _config = config;


    public IConnection GetConnection()
    {
        if (_connection != null && _connection.IsOpen) return _connection;


        var factory = new ConnectionFactory
        {
            HostName = _config["RabbitMQ:HostName"],
            UserName = _config["RabbitMQ:UserName"],
            Password = _config["RabbitMQ:Password"],
            DispatchConsumersAsync = true
        };
        _connection = factory.CreateConnection();
        return _connection;
    }
}