using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModulbankInternship.Infrastructure.Rabbit.Models;

namespace ModulbankInternship.Infrastructure.Rabbit.Outbox;

public class OutboxMessageConfiguration: IEntityTypeConfiguration<OutboxMessageModel>
{
    public void Configure(EntityTypeBuilder<OutboxMessageModel> builder)
    {
        builder.HasKey(x => x.MessageId);
        builder.Property(x => x.Payload).IsRequired();
        builder.Property(x => x.RoutingKey).IsRequired();
        builder.Property(x => x.EventType).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.IsDispatched);
        builder.Property(x => x.DispatchedAt);
        builder.Property(x => x.AttemptCount);
        builder.Property(x => x.LastError);
    }
}