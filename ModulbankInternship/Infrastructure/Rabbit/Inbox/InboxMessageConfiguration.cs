using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModulbankInternship.Infrastructure.Rabbit.Inbox;

public class InboxMessageConfiguration: IEntityTypeConfiguration<InboxConsumedModel>
{
    public void Configure(EntityTypeBuilder<InboxConsumedModel> builder)
    {
        builder.HasKey(x => x.EventId);
        builder.Property(x => x.Handler).HasMaxLength(200);
    }
}