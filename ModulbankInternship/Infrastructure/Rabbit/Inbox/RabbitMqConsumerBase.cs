using System.Text;
using Microsoft.EntityFrameworkCore;
using ModulbankInternship.Infrastructure.Rabbit.Inbox.Quarantine;
using ModulbankInternship.Infrastructure.Rabbit.Interfaces;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ModulbankInternship.Infrastructure.Rabbit.Inbox;

public abstract class RabbitMqConsumerBase<TEvent> : BackgroundService
    where TEvent : IEvent
{
    private readonly IModel channel;
    private readonly string queueName;
    private readonly ILogger<RabbitMqConsumerBase<TEvent>> logger;
    protected readonly ApplicationDbContext DbContext;

    protected RabbitMqConsumerBase(IModel channel, ApplicationDbContext dbContext, ILogger<RabbitMqConsumerBase<TEvent>> logger, string queueName)
    {
        this.channel = channel;
        this.DbContext = dbContext;
        this.queueName = queueName;
        this.logger = logger;

        this.channel.QueueDeclare(
            queue: this.queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    /// <summary>
    /// Реализация бизнес-логики конкретного события
    /// </summary>
    protected abstract Task HandleEventAsync(TEvent evt, string routingKey, CancellationToken ct);

    protected virtual bool IsSupportedVersion(string version) => version == "1.0";

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                var envelope = JsonSerializer.Deserialize<EventEnvelope<TEvent>>(message);

                if (envelope is null || envelope.Payload is null)
                {
                    throw new InvalidDataException("Cannot deserialize envelope or payload");
                }

                if (!IsSupportedVersion(envelope.Meta.Version))
                {
                    throw new NotSupportedException($"Unsupported version {envelope.Meta.Version}");
                }

                var consumerName = GetType().Name;
                var alreadyProcessed = await DbContext.InboxConsumed
                    .FirstOrDefaultAsync(x => x.EventId == envelope.MessageId && x.Handler == consumerName,
                        stoppingToken);

                if (alreadyProcessed != null)
                {
                    channel.BasicAck(ea.DeliveryTag, multiple: false);
                    return;
                }

                var strategy = DbContext.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    await using var trx = await DbContext.Database.BeginTransactionAsync(stoppingToken);

                    await HandleEventAsync(envelope.Payload, ea.RoutingKey, stoppingToken);

                    await DbContext.InboxConsumed.AddAsync(new InboxConsumedModel
                    {
                        EventId = envelope.MessageId,
                        Handler = GetType().Name,
                        ProcessedAt = DateTime.UtcNow
                    }, stoppingToken);

                    await DbContext.SaveChangesAsync(stoppingToken);
                    await trx.CommitAsync(stoppingToken);

                    logger.LogInformation(
                        "Consumed {EventType} EventId={EventId} CorrelationId={CorrelationId} RoutingKey={RoutingKey}",
                        typeof(TEvent).Name, envelope.MessageId, envelope.Meta.CorrelationId, ea.RoutingKey);

                    channel.BasicAck(ea.DeliveryTag, multiple: false);
                });
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to process message on {Queue}", queueName);
                channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);

                await DbContext.InboxDeadLetter.AddAsync(new InboxDeadLetterModel
                {
                    MessageId = Guid.NewGuid(),
                    ReceivedAt = DateTime.UtcNow,
                    Handler = GetType().Name,
                    Payload = message,
                    Error = ex.Message
                }, stoppingToken);
                await DbContext.SaveChangesAsync(stoppingToken);
            }
        };

        channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        return Task.CompletedTask;
    }
}