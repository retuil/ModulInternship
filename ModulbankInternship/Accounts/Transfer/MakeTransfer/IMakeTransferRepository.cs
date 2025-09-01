using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Accounts.Transfer;

public interface IMakeTransferRepository
{
    public Task MakeTransfer(TransactionModel debitTransaction, TransactionModel creditTransaction);
}