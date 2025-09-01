namespace ModulbankInternship.Infrastructure.Rabbit.Models;

public class OutboxMessageModel
{
    public Guid MessageId { get; set; } = Guid.NewGuid();
    public string EventType { get; set; } = default!;
    public string RoutingKey { get; set; } = default!;
    public string Payload { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DispatchedAt { get; set; }
    public int AttemptCount { get; set; } = 0;
    public string? LastError { get; set; }
    public bool IsDispatched { get; set; }
}
