namespace ModulbankInternship.Infrastructure.Rabbit.Outbox.Events;

public record MoneyDebitedEvent(Guid EventId, DateTime OccuredAt, Guid AccountId, decimal Amount, string Currency, Guid OperationId, string Reason);