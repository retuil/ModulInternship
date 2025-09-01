using ModulbankInternship.Infrastructure.Rabbit.DTO;
using ModulbankInternship.Infrastructure.Rabbit.Interfaces;

namespace ModulbankInternship.Infrastructure.Rabbit.Inbox;

public class EventEnvelope<TEvent> where TEvent : IEvent
{
    public Guid MessageId { get; init; }
    public DateTime OccurredAt { get; init; }
    public EventMeta Meta { get; init; } = default!;
    public TEvent Payload { get; init; } = default!;
}