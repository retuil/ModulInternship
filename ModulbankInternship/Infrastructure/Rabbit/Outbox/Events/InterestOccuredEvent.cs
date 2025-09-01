namespace ModulbankInternship.Infrastructure.Rabbit.Outbox.Events;

public record InterestOccuredEvent(
    Guid EventId,
    DateTime OccuredAt,
    Guid AccountId,
    DateTime PeriodFrom,
    DateTime PeriodTo,
    decimal Amount
    );