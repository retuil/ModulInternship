using ModulbankInternship.Infrastructure.Rabbit.DTO;
using ModulbankInternship.Infrastructure.Rabbit.Events;

namespace ModulbankInternship.Infrastructure.Rabbit.Inbox.Antifraud;

public record ClientUnblockedEvent(Guid EventId, DateTime OccurredAt, EventMeta Meta, Guid ClientId)
    : ClientBlockEventBase(EventId, OccurredAt, Meta, ClientId);