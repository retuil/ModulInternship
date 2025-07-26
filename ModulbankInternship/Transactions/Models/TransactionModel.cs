using ModulbankInternship.Transactions.Enums;

namespace ModulbankInternship.Transactions;

public class TransactionModel
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid? CounterpartyAccountId { get; set; }
    public decimal Amount { get; set; }
    public ETransactionType Type { get; set; }
    public string Currency { get; set; }
    public DateTime DateTime { get; set; }
    public string Description { get; set; }
    public string? Message { get; set; }
}