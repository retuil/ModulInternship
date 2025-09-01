using ModulbankInternship.Accounts.AntifraudBlocks;
using ModulbankInternship.Infrastructure.Rabbit.Events;
using RabbitMQ.Client;

namespace ModulbankInternship.Infrastructure.Rabbit.Inbox.Antifraud;

public class AntifraudConsumer(
    IModel channel,
    IServiceScopeFactory scopeFactory,
    ILogger<AntifraudConsumer> logger)
    : RabbitMqConsumerBase<ClientBlockEventBase>(channel,
        scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>(),
        logger,
        "account.antifraud")
{
    private readonly IBlockAccountRepository accountsRepository = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IBlockAccountRepository>();

    protected override async Task HandleEventAsync(ClientBlockEventBase evt, string routingKey, CancellationToken ct)
    {
        switch (evt)
        {
            case ClientBlockedEvent blocked:
            {
                await accountsRepository.BlockAccountAsync(blocked.ClientId);
                break;
            }
            case ClientUnblockedEvent unblocked:
            {
                await accountsRepository.UnblockAccountAsync(unblocked.ClientId);
                break;
            }
            default:
            {
                throw new NotSupportedException($"Unsupported event type {evt.GetType().Name}");
            }
        }
    }

    
}