using ModulbankInternship.Accounts.Transactions.Domain.Enums;

namespace ModulbankInternship.Accounts.Transactions.MakeTransaction;

public class NewTransactionRequest
{
    /// <summary>Id счета на котором выполняется транзакция</summary>
    public Guid AccountId { get; set; }
    
    /// <summary>Id счета на котором выполняется связанная с данной транзакция (например целевой счет при переводе)</summary>
    public Guid? CounterpartyAccountId { get; set; }
    
    /// <summary>Сумма по транзакции</summary>
    public decimal Amount { get; set; }
    
    /// <summary>Валюта в которой выполняется транзакция</summary>
    public string Currency { get; set; }

    /// <summary>Тип транзакции</summary>
    public ETransactionType TransactionType { get; set; }
    
    /// <summary>Краткое описание транзакции</summary>
    public string Description { get; set; }

}