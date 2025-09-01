using ModulbankInternship.Infrastructure.Rabbit.DTO;
using ModulbankInternship.Infrastructure.Rabbit.Interfaces;

namespace ModulbankInternship.Infrastructure.Rabbit.Inbox.Antifraud;

public record ClientBlockEventBase(
    Guid EventId,
    DateTime OccurredAt,
    EventMeta Meta,
    Guid ClientId
) : IEvent;