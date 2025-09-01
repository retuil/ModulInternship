namespace ModulbankInternship.Infrastructure.Rabbit.Outbox.Events;

public record TransferCompletedEvent(
    Guid EventId,
    DateTime OccuredAt,
    Guid SourceAccountId,
    Guid DestinationAccountId,
    decimal Amount,
    string Currency,
    Guid TransferId
    );