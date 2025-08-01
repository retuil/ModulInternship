using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions.Enums;

namespace ModulbankInternship.Transactions.Models;

public class TransactionModel: IModel
{
    /// <summary>Id транзакции</summary>
    public Guid Id { get; set; }
    
    /// <summary>Id счета, к которому привязана транзакция</summary>
    public Guid AccountId { get; set; }
    
    /// <summary>Id счета, к которому принадлежит связанная с данной транзакция</summary>
    public Guid? CounterpartyAccountId { get; set; }
    
    /// <summary>Сумма по транзакции</summary>
    public decimal Amount { get; set; }
    
    /// <summary>Тип транзакции</summary>
    public ETransactionType Type { get; set; }
    
    /// <summary>Валюта транзакции</summary>
    public string Currency { get; set; }
    
    /// <summary>Дата и время совершения транзакции</summary>
    public DateTime DateTime { get; set; }
    
    /// <summary>Описание транзакции</summary>
    public string Description { get; set; }
    
    /// <summary>Транзакция существует или отменена</summary>
    public bool IsExist { get; set; } = true;
}