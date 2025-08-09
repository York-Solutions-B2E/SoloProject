using System.Text;
using System.Text.Json;
using App.Shared.Dtos;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WebApi.CommunicationDbContext;
using WebApi.Entities;

namespace WebApi.Services
{
    public class CommunicationEventConsumer : BackgroundService
{
    private readonly IServiceProvider _services;
    

    public CommunicationEventConsumer(IServiceProvider services)
        {
            _services = services;
        }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory() { HostName = "rabbitmq"};
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "communication_events",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (sender, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var evt = JsonSerializer.Deserialize<CommunicationEventDto>(json); //deserialize DTO to be updated

            if (evt != null)
            {
                using var scope = _services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var comm = await db.Communications.FindAsync(evt.CommunicationId);
                if (comm != null)
                {
                    comm.CurrentStatus = evt.EventCode;
                    comm.LastUpdatedUtc = DateTime.UtcNow;
                    db.CommunicationStatusHistories.Add(new CommunicationStatusHistory //Add new history in list of histories
                    {
                        
                        Communication = comm,
                        CommunicationId = comm.Id,
                        StatusCode = evt.EventCode,
                        OccurredUtc = evt.PublishedAt
                    });
                    await db.SaveChangesAsync();
                }
            }

            await channel.BasicAckAsync(ea.DeliveryTag, false); //Acknowledge message to be consumed
        };

        await channel.BasicConsumeAsync("communication_events", false, consumer, cancellationToken: stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken); // Keep service alive
        }
    }
}

}