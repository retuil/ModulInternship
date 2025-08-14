using Microsoft.EntityFrameworkCore;
using ModulbankInternship.Accounts.Models;
using ModulbankInternship.Accounts.Repository;
using ModulbankInternship.Transactions.Models;
using ModulbankInternship.Transactions.Repository;

namespace ModulbankInternship.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<AccountModel> Accounts { get; set; }
    public DbSet<TransactionModel> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        
        modelBuilder.Entity<AccountModel>()
            .Property<uint>("Version")
            .IsConcurrencyToken()
            .HasColumnName("xmin");

        base.OnModelCreating(modelBuilder);
    }
}