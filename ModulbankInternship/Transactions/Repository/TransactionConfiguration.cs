using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModulbankInternship.Accounts.Models;
using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Transactions.Repository;

public class TransactionConfiguration : IEntityTypeConfiguration<TransactionModel>
{
    public void Configure(EntityTypeBuilder<TransactionModel> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(t => t.Amount)
            .HasPrecision(18, 2);

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.Property(t => t.Type);

        builder.Property(t => t.DateTime);

        builder.Property(t => t.IsExist);
        
        builder.HasOne<AccountModel>()
            .WithMany()
            .HasForeignKey(t => t.CounterpartyAccountId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}