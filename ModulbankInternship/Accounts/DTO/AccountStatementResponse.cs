using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Accounts.DTO;

public class AccountStatementResponse
{
    public DateTime CreationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public IEnumerable<TransactionModel> Transactions { get; set; }
}