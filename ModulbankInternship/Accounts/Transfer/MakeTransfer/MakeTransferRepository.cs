using Microsoft.EntityFrameworkCore;
using ModulbankInternship.Accounts.Domain.Exceptions;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Rabbit.Outbox;
using ModulbankInternship.Transactions.MakeTransaction;
using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Accounts.Transfer.MakeTransfer;

public class MakeTransferRepository(ApplicationDbContext context, IMakeTransactionRepository transactionRep): IMakeTransferRepository
{
    public async Task MakeTransfer(TransactionModel debitTransaction, TransactionModel creditTransaction)
    {
        if (debitTransaction.Amount != creditTransaction.Amount)
        {
            throw new InvalidOperationException("The amounts on the transfer transactions do not match");
        }

        if (debitTransaction.Currency != creditTransaction.Currency)
        {
            throw new InvalidOperationException("The currency on the transfer transactions do not match");
        }

        

        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var bdTransaction =
                await context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                var debitAccount = await context.Accounts
                    .FirstOrDefaultAsync(a => a.Id == debitTransaction.AccountId);
                var creditAccount = await context.Accounts
                    .FirstOrDefaultAsync(a => a.Id == creditTransaction.AccountId);
        
                CheckAccountExist(debitAccount, debitTransaction.AccountId);
                CheckAccountExist(creditAccount, creditTransaction.AccountId);
        
                var startBalanceDebitAccount = debitAccount!.Balance;
                var startBalanceCreditAccount = creditAccount!.Balance;
                
                await transactionRep.InternalAddTransaction(debitTransaction, debitAccount);
                await transactionRep.InternalAddTransaction(creditTransaction, creditAccount);
                
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

                await InternalAddTransfer(debitTransaction, creditTransaction);

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
                catch (Exception rollbackEx)
                {
                    // Транзакция уже завершена / Rollback не нужен — проглатываем
                }
            }
            catch
            {
                try
                {
                    if (context.Database.CurrentTransaction != null)
                    {
                        await bdTransaction.RollbackAsync();
                    }
                }
                catch (Exception rollbackEx)
                {
                    // Транзакция уже завершена / Rollback не нужен — проглатываем
                }
                throw;
            }
        });
    }

    internal async Task InternalAddTransfer(TransactionModel debitTransaction, TransactionModel creditTransaction)
    {
        var transfer = new TransferModel()
        {
            Amount = debitTransaction.Amount,
            CreditTransactionId = creditTransaction.Id,
            DebitTransactionId = debitTransaction.Id,
            Currency = debitTransaction.Currency,
            SourceAccountId = creditTransaction.AccountId,
            DestinationAccountId = debitTransaction.AccountId,
            DateTime = DateTime.UtcNow
        };
        context.Transfers.Add(transfer);
        await context.OutboxMessages.AddAsync(OutboxMessageCreator.TransferCompleted(transfer));
        await context.SaveChangesAsync();
    }

    private static void CheckAccountExist(AccountModel? account, Guid accountId)
    {
        if (account is null || !account.IsExist)
        {
            throw new AccountNotFoundException(accountId);
        }
        
        if (account.IsFrozen)
        {
            throw new AccountBlockedException(accountId);
        }
    }
}