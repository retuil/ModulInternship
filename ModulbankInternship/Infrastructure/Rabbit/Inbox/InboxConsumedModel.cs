namespace ModulbankInternship.Infrastructure.Rabbit.Inbox;

public class InboxConsumedModel
{
    public Guid EventId { get; set; }
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
    public string? Handler { get; set; }
}
