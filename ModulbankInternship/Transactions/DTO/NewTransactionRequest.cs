using ModulbankInternship.Transactions.Enums;

namespace ModulbankInternship.Transactions.DTO;

public class NewTransactionRequest
{
    public Guid walletId { get; set; }
    public Guid? counterpartyAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }

    public ETransactionType TransactionType { get; set; }
    public string Description { get; set; }

}