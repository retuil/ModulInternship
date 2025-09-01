using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Transactions.MakeTransaction;

public interface IMakeTransactionRepository
{
    public Task<Guid?> AddAsync(TransactionModel transaction);

    internal Task InternalAddTransaction(TransactionModel transaction, AccountModel account);
}