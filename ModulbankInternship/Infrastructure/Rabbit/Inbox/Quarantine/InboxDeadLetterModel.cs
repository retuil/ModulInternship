namespace ModulbankInternship.Infrastructure.Rabbit.Inbox.Quarantine;

public class InboxDeadLetterModel
{
    public Guid MessageId { get; set; }
    public DateTime ReceivedAt { get; set; }
    public string Handler { get; set; } = null!;
    public string Payload { get; set; } = null!;
    public string Error { get; set; } = null!;
}