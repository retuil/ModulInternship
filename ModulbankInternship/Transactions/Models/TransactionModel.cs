using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions.Enums;

namespace ModulbankInternship.Transactions;

public class TransactionModel: IModel
{
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public Guid? CounterpartyWalletId { get; set; }
    public decimal Amount { get; set; }
    public ETransactionType Type { get; set; }
    public string Currency { get; set; }
    public DateTime DateTime { get; set; }
    public string Description { get; set; }
    public bool IsExist { get; set; } = true;
}