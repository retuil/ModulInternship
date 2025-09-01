using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModulbankInternship.Accounts.Domain.Models;

namespace ModulbankInternship.Accounts.Domain.Configurations;
    

public class AccountConfiguration : IEntityTypeConfiguration<AccountModel>
{
    public void Configure(EntityTypeBuilder<AccountModel> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(a => a.Balance)
            .HasPrecision(18, 2);

        builder.Property(a => a.InterestRate)
            .HasPrecision(5, 2);

        builder.Property(a => a.OpeningDate);
        
        builder.Property(a => a.ClosingDate);
        
        builder.Property(a => a.AccountType);

        builder.HasMany(a => a.Transactions)
            .WithOne()
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(a => a.Version)
            .HasColumnName("xmin")
            .HasColumnType("xid")
            .ValueGeneratedOnAddOrUpdate()
            .IsRowVersion();   
    }
}
