using App.Shared.Dtos;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace WebApi.Services
{

public class CommunicationEventPublisher
{
    private readonly string _queueName = "communication_events";

    public async Task PublishAsync(CommunicationEventDto evt)
    {
        var factory = new ConnectionFactory() { HostName = "rabbitmq" }; // Docker Compose service name
        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var message = JsonSerializer.Serialize(evt);
        var body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: _queueName,
            mandatory: true,
            basicProperties: new BasicProperties { Persistent = true },
            body: body);
        }
    }
}