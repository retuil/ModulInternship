using System.Transactions;
using ModulbankInternship.Transactions;
using ModulbankInternship.Transactions;

namespace ModulbankInternship.Account;

public class WalletModel
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public EWalletType Type { get; set; }
    public string Currency { get; set; }
    public decimal Balance { get; set; }
    public decimal? InterestRate { get; set; }
    public DateTime OpeningDate { get; set; }
    public DateTime? ClosingDate { get; set; }
    public IEnumerable<TransactionModel> Transactions { get; set; }
}