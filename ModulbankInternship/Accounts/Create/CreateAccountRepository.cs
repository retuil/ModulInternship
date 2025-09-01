using Microsoft.EntityFrameworkCore;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Rabbit.Outbox;

namespace ModulbankInternship.Accounts.Create;

public class CreateAccountRepository(ApplicationDbContext context): ICreateAccountRepository
{
    public async Task<Guid?> AddAsync(AccountModel account)
    {
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var bdTransaction =
                await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                context.Accounts.Add(account);
                context.OutboxMessages.Add(OutboxMessageCreator.AccountOpened(account));
                await context.SaveChangesAsync();
                await bdTransaction.CommitAsync();
            }
            catch (Exception e)
            {
                await bdTransaction.RollbackAsync();
            }
        });
        return account.Id;
        
    }
}