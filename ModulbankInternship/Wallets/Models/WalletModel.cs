using System.Transactions;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions;
using ModulbankInternship.Transactions;

namespace ModulbankInternship.Account;

public class WalletModel: IModel
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public EWalletType WalletType { get; set; }
    public string Currency { get; set; }
    public decimal Balance { get; set; }
    public decimal? InterestRate { get; set; }
    public bool IsExist { get; set; }
    public DateTime OpeningDate { get; set; }
    public DateTime? ClosingDate { get; set; }
    public List<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();
}