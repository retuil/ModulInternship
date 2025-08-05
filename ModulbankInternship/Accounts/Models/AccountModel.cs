using ModulbankInternship.Accounts.Enums;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Accounts.Models;

public class AccountModel: IModel
{
    /// <summary>Id счета</summary>
    public Guid Id { get; set; }
    
    /// <summary>Id владельца счета</summary>
    public Guid OwnerId { get; set; }
    
    /// <summary>Тип счета</summary>
    public EAccountType AccountType { get; set; }
    
    /// <summary>Валюта счета</summary>
    public string Currency { get; set; }
    
    /// <summary>Текущий баланс счета</summary>
    public decimal Balance { get; set; }
    
    /// <summary>Процентная ставка по счету</summary>
    public decimal? InterestRate { get; set; }
    
    /// <summary>Открыт ли счет на данный момент</summary>
    public bool IsExist { get; set; } = true;
    
    /// <summary>Дата открытия счета</summary>
    public DateTime OpeningDate { get; set; }
    
    /// <summary>Дата закрытия счета</summary>
    public DateTime? ClosingDate { get; set; }
    
    /// <summary>Список транзакций по счету</summary>
    public List<TransactionModel> Transactions { get; set; } = [];
}