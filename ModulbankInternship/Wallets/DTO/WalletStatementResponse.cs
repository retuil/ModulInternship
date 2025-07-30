using ModulbankInternship.Transactions;
using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Account;

public class WalletStatementResponse
{
    public DateTime CreationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public IEnumerable<TransactionModel> Transactions { get; set; }
}