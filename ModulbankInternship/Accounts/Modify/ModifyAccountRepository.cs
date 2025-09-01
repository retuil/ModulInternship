using Microsoft.EntityFrameworkCore;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Rabbit.Outbox;

namespace ModulbankInternship.Accounts.Modify;

public class ModifyAccountRepository(ApplicationDbContext context): IModifyAccountRepository
{
    public async Task UpdateAsync(AccountModel account)
    {
        context.Accounts.Update(account);
        await context.SaveChangesAsync();
    }
    
    public async Task AccrueInterestsAsync()
    {
        var accounts = await context.Accounts
            .Where(a => a.Balance > 0 && a.InterestRate.HasValue && a.InterestRate > 0 && a.IsExist && !a.IsFrozen)
            .ToListAsync();
        foreach (var account in accounts)
        {
            var startBalance = account.Balance;

            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var dbTransaction =
                    await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
                try
                {
                    await context.Database.ExecuteSqlRawAsync("CALL accrue_interest({0})", account.Id);
                    var newBalance = account.Balance;
                    await context.OutboxMessages.AddAsync(OutboxMessageCreator.InterestAccrued(
                        account.Id,
                        DateTime.UtcNow.AddDays(-1),
                        DateTime.UtcNow,
                        newBalance - startBalance));
                    await context.SaveChangesAsync();
                    await dbTransaction.CommitAsync();
                }
                catch
                {
                    await dbTransaction.RollbackAsync();
                    throw;
                }
            });
        }
        
        
    }
}