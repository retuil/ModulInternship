using ModulbankInternship.Infrastructure.Rabbit.Events;
using ModulbankInternship.Infrastructure.Rabbit.Inbox.Antifraud;
using RabbitMQ.Client;

namespace ModulbankInternship.Infrastructure.Rabbit.Inbox.Audit;

public class AuditConsumer(IModel channel, IServiceScopeFactory scopeFactory, ILogger<AuditConsumer> logger)
    : RabbitMqConsumerBase<AuditEvent>(
        channel,
        scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>(),
        logger,
        "account.audit")
{
    protected override async Task HandleEventAsync(AuditEvent evt, string routingKey, CancellationToken ct)
    {

        await DbContext.InboxConsumed.AddAsync(new InboxConsumedModel()
        {
            EventId = evt.EventId,
            ProcessedAt = DateTime.Now,
            Handler = null
        }, ct);
        await DbContext.SaveChangesAsync(ct);
        
        var latency = DateTime.UtcNow - evt.OccurredAt;
        
        logger.LogInformation(
            "Consumed event {@EventType} | EventId={EventId} | CorrelationId={CorrelationId} | Latency={Latency}ms | RoutingKey={RoutingKey}",
            nameof(ClientBlockEventBase),
            evt.EventId,
            evt.Meta.CorrelationId,
            latency.TotalMilliseconds,
            routingKey
        );
    }
    
}