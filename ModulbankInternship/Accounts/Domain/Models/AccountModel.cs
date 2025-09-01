using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ModulbankInternship.Accounts.Domain.Enums;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Accounts.Domain.Models;

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
    
    public bool IsFrozen { get; set; }
    
    /// <summary>Дата открытия счета</summary>
    public DateTime OpeningDate { get; set; }
    
    /// <summary>Дата закрытия счета</summary>
    public DateTime? ClosingDate { get; set; }
    
    /// <summary>Список транзакций по счету</summary>
    public List<TransactionModel> Transactions { get; set; } = [];
    
    [Timestamp]
    [Column("xmin", TypeName = "xid")]
    public uint Version { get; set; }
}