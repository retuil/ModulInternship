using ModulbankInternship.Infrastructure.Rabbit.DTO;
using ModulbankInternship.Infrastructure.Rabbit.Interfaces;

namespace ModulbankInternship.Infrastructure.Rabbit.Events;

public record AuditEvent(
    Guid EventId,
    DateTime OccurredAt,
    EventMeta Meta)
    : IEvent;