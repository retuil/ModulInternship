using Microsoft.EntityFrameworkCore;
using ModulbankInternship.Accounts.Models;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Exceptions;
using ModulbankInternship.Transactions.Enums;
using ModulbankInternship.Transactions.Interfaces;
using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Transactions;

public class TransactionsRepository(
    ApplicationDbContext context)
    : ITransactionsRepository
{
    public async Task<TransactionModel?> GetByIdAsync(Guid id)
    {
        return await context.Transactions.FindAsync(id);
    }

    public async Task DeleteAsync(Guid id)
    {
        var account = await context.Accounts.FindAsync(id);
        if (account != null)
        {
            account.IsExist = false;
            context.Accounts.Update(account);
            await context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(TransactionModel transaction)
    {
        context.Transactions.Update(transaction);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TransactionModel>> GetAllByAccountIdAsync(Guid accountId)
    {
        return await context.Transactions
            .Where(t => t.AccountId == accountId)
            .ToListAsync();
    }

    public async Task<Guid?> AddAsync(TransactionModel transaction)
    {
        await using var bdTransaction = await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

        try
        {
            var account = await context.Accounts
                .FirstOrDefaultAsync(a => a.Id == transaction.AccountId);

            CheckAccountExist(account, transaction.AccountId);

            AddTransaction(transaction, account);
            
            await context.SaveChangesAsync();

            await bdTransaction.CommitAsync();
        }
        catch
        {
            await bdTransaction.RollbackAsync();
            throw;
        }
        
        
        return transaction.Id;
    }

    public async Task MakeTransfer(TransactionModel debitTransaction, TransactionModel creditTransaction)
    {
        if (debitTransaction.Amount != creditTransaction.Amount)
        {
            throw new InvalidOperationException("The amounts on the transfer transactions do not match");
        }
        
        await using var bdTransaction = await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

        var debitAccount = await context.Accounts
            .FirstOrDefaultAsync(a => a.Id == debitTransaction.AccountId);
        var creditAccount = await context.Accounts
            .FirstOrDefaultAsync(a => a.Id == creditTransaction.AccountId);
        
        CheckAccountExist(debitAccount, debitTransaction.AccountId);
        CheckAccountExist(creditAccount, creditTransaction.AccountId);
        
        var startBalanceDebitAccount = debitAccount.Balance;
        var startBalanceCreditAccount = creditAccount.Balance;

        try
        {
            AddTransaction(debitTransaction, debitAccount);
            AddTransaction(creditTransaction, creditAccount);

            if (debitAccount.Balance != startBalanceDebitAccount + debitTransaction.Amount ||
                creditAccount.Balance != startBalanceCreditAccount - creditTransaction.Amount)
            {
                throw new InvalidOperationException("The final balance does not comply with the rules.");
            }
            
            await context.SaveChangesAsync();

            await bdTransaction.CommitAsync();
        }
        catch
        {
            await bdTransaction.RollbackAsync();
            throw;
        }
    }

    private void AddTransaction(TransactionModel transaction, AccountModel account)
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
                throw new NotImplementedException();
            }
        }

        if (account.Balance < 0)
        {
            throw new InvalidOperationException("The final balance does not comply with the rules.");
        }
            
        context.Transactions.Add(transaction);
    }

    private static void CheckAccountExist(AccountModel? account, Guid accountId)
    {
        if (account == null || !account.IsExist)
        {
            throw new ResourceNotFoundException($"Account Id: {accountId} not found or closed");
        }
    }
}