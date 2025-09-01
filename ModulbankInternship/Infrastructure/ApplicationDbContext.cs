using Microsoft.EntityFrameworkCore;
using ModulbankInternship.Accounts.Domain.Configurations;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Accounts.Transactions.Domain;
using ModulbankInternship.Accounts.Transfer.Domain;
using ModulbankInternship.Infrastructure.Rabbit.Inbox;
using ModulbankInternship.Infrastructure.Rabbit.Inbox.Quarantine;
using ModulbankInternship.Infrastructure.Rabbit.Models;
using ModulbankInternship.Infrastructure.Rabbit.Outbox;
using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<AccountModel> Accounts { get; set; }
    public DbSet<TransactionModel> Transactions { get; set; }
    public DbSet<TransferModel> Transfers { get; set; }
    public DbSet<OutboxMessageModel> OutboxMessages { get; set; }
    public DbSet<InboxConsumedModel> InboxConsumed { get; set; }
    
    public DbSet<InboxDeadLetterModel> InboxDeadLetter { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new TransferConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new InboxDeadLetterConfiguration());
        
        modelBuilder.Entity<AccountModel>()
            .Property<uint>("Version")
            .IsConcurrencyToken()
            .HasColumnName("xmin");

        base.OnModelCreating(modelBuilder);
    }
}