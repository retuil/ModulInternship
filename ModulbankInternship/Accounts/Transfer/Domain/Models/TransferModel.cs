using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Transactions.Models;

public class TransferModel: IModel
{
    /// <summary>Id перевода</summary>
    public Guid Id { get; set; }
    
    /// <summary>Id счета отправителя</summary>
    public Guid SourceAccountId { get; set; }
    
    /// <summary>Id счета получателя</summary>
    public Guid DestinationAccountId { get; set; }
    
    /// <summary>Id дебетовой транзакции</summary>
    public Guid DebitTransactionId { get; set; }
    
    /// <summary>Id кредитовой транзакции</summary>
    public Guid CreditTransactionId { get; set; }
    
    /// <summary>Сумма перевода</summary>
    public decimal Amount { get; set; }
    
    /// <summary>Валюта, в которой осуществляется перевод</summary>
    public string Currency { get; set; }
    
    /// <summary>Время осуществления перевода</summary>
    public DateTime DateTime { get; set; }
    
    /// <summary>Перевод существует или отменен</summary>
    public bool IsExist { get; set; } = true;
}