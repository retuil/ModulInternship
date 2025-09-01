using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModulbankInternship.Accounts.Domain.Models;

namespace ModulbankInternship.Infrastructure.Rabbit.Inbox.Quarantine;

public class InboxDeadLetterConfiguration: IEntityTypeConfiguration<InboxDeadLetterModel>
{
    public void Configure(EntityTypeBuilder<InboxDeadLetterModel> builder)
    {
        builder.HasKey(x => new { x.MessageId, x.Handler });
        builder.Property(x => x.Error);
        builder.Property(x => x.Payload);
    }
}