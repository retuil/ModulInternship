namespace ModulbankInternship.Infrastructure.Rabbit.Outbox.Events;

public record MoneyCreditedEvent(Guid EventId, DateTime OccuredAt, Guid AccountId, decimal Amount, string Currency, Guid OperationId);