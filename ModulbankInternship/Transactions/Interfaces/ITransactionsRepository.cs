using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Transactions.Interfaces;

public interface ITransactionsRepository : IRepository<TransactionModel>
{
    public Task<IEnumerable<TransactionModel>> GetAllByAccountIdAsync(Guid accountId);

    public Task MakeTransfer(TransactionModel debitTransaction, TransactionModel creditTransaction);
}