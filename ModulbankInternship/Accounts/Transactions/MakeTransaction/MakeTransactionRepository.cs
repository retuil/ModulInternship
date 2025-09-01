using Microsoft.EntityFrameworkCore;
using ModulbankInternship.Accounts.Domain.Exceptions;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Accounts.Transactions.Domain.Enums;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Rabbit.Outbox;
using ModulbankInternship.Transactions.MakeTransaction;
using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Accounts.Transactions.MakeTransaction;

public class MakeTransactionRepository(ApplicationDbContext context): IMakeTransactionRepository
{
    public async Task<Guid?> AddAsync(TransactionModel transaction)
    {
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var bdTransaction =
                await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                var account = await context.Accounts
                    .FirstOrDefaultAsync(a => a.Id == transaction.AccountId);;

                if (account is null || !account.IsExist)
                {
                    throw new AccountNotFoundException(transaction.AccountId);
                }
        
                if (account.IsFrozen)
                {
                    throw new AccountBlockedException(transaction.AccountId);
                }
            
            
                await InternalAddTransaction(transaction, account);
                try
                {
                    await context.SaveChangesAsync();

                    await bdTransaction.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    await bdTransaction.RollbackAsync();
                    throw;
                }
            }
            catch
            {
                await bdTransaction.RollbackAsync();
                throw;
            }
        });
        return transaction.Id;
    }
    
    public async Task InternalAddTransaction(TransactionModel transaction, AccountModel account)
    {
        switch (transaction.Type)
        {
            case ETransactionType.Credit:
            {
                if (account.Balance < transaction.Amount)
                {
                    throw new InvalidOperationException($"Insufficient funds in the sender's account. Id: {account.Id}");
                }
                account.Balance -= transaction.Amount;
                break;
            }
            case ETransactionType.Debit:
            {
                account.Balance += transaction.Amount;
                break;
            }
            default:
            {
                throw new NotSupportedException();
            }
        }

        if (account.Balance < 0)
        {
            throw new InvalidOperationException("The final balance does not comply with the rules.");
        }
        
        await context.Transactions.AddAsync(transaction);
        await context.OutboxMessages.AddAsync(OutboxMessageCreator.NewTransaction(transaction));
    }
}