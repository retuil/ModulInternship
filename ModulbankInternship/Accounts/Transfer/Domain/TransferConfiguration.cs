using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Accounts.Transfer.Domain;

public class TransferConfiguration: IEntityTypeConfiguration<TransferModel>
{
    public void Configure(EntityTypeBuilder<TransferModel> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(t => t.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(t => t.Amount)
            .HasPrecision(18, 2);

        builder.Property(t => t.DateTime);

        builder.Property(t => t.IsExist);
        
        builder.HasOne<AccountModel>()
            .WithMany()
            .HasForeignKey(t => t.SourceAccountId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<AccountModel>()
            .WithMany()
            .HasForeignKey(t => t.DestinationAccountId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<TransactionModel>()
            .WithOne()
            .HasForeignKey<TransferModel>(t => t.DebitTransactionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<TransactionModel>()
            .WithOne()
            .HasForeignKey<TransferModel>(t => t.CreditTransactionId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}