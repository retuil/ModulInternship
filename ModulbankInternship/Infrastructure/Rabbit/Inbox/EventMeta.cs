namespace ModulbankInternship.Infrastructure.Rabbit.DTO;

public record EventMeta(
    string Version,
    string Source,
    Guid CorrelationId,
    Guid? CausationId,
    int RetryCount
);