using System.Diagnostics;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ModulbankInternship.Infrastructure.Rabbit.Outbox.publisher;
using RabbitMQ.Client;

namespace ModulbankInternship.Infrastructure.Rabbit.Outbox;

public class OutboxDispatcher(
    IServiceProvider sp,
    ILogger<OutboxDispatcher> logger,
    IMessagePublisher publisher) : BackgroundService
{
    private const int BatchSize = 20;
    private readonly IMessagePublisher _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
    private const string Exchange = "account.events";
    private readonly TimeSpan pollDelay = TimeSpan.FromSeconds(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var pending = await db.OutboxMessages
                    .Where(m => !m.IsDispatched)
                    .OrderBy(m => m.CreatedAt)
                    .Take(BatchSize)
                    .ToListAsync(stoppingToken);

                if (!pending.Any())
                {
                    await Task.Delay(pollDelay, stoppingToken);
                    continue;
                }

                foreach (var msg in pending)
                {
                    var sw = Stopwatch.StartNew();

                    try
                    {
                        var body = string.IsNullOrWhiteSpace(msg.Payload)
                            ? Array.Empty<byte>()
                            : Encoding.UTF8.GetBytes(msg.Payload.Trim());

                        IBasicProperties? props = null;

                        msg.AttemptCount += 1;

                        _publisher.Publish(Exchange, msg.RoutingKey, props!, body);

                        msg.IsDispatched = true;
                        msg.DispatchedAt = DateTime.UtcNow;
                        msg.LastError = null;

                        sw.Stop();
                        logger.LogInformation(
                            "Published event {EventType} Id={MessageId} Attempts={Attempts} LatencyMs={Latency} RoutingKey={RoutingKey}",
                            msg.EventType, msg.MessageId, msg.AttemptCount, sw.ElapsedMilliseconds, msg.RoutingKey);
                    }
                    catch (Exception exMsg)
                    {
                        logger.LogError(exMsg, "Failed publishing outbox message {MessageId}", msg.MessageId);
                        msg.LastError = exMsg.ToString();
                        await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                    }
                }

                await db.SaveChangesAsync(stoppingToken);
            }
            catch (Exception exLoop)
            {
                logger.LogError(exLoop, "Error while dispatching outbox batch — will retry");
                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }
        }
    }
}
