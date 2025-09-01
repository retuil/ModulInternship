using ModulbankInternship.Accounts.Domain.Enums;

namespace ModulbankInternship.Infrastructure.Rabbit.Outbox.Events;

public record AccountOpenedEvent(
    Guid EventId,
    DateTime OccuredAt,
    Guid AccountId,
    Guid OwnerId,
    string Currency,
    EAccountType Type
    );