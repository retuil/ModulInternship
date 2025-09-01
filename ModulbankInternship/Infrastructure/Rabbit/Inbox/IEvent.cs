using ModulbankInternship.Infrastructure.Rabbit.DTO;

namespace ModulbankInternship.Infrastructure.Rabbit.Interfaces;

public interface IEvent
{
    Guid EventId { get; }
    DateTime OccurredAt { get; }
    EventMeta Meta { get; }
}