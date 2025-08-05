using ModulbankInternship.Account;
using ModulbankInternship.Accounts.Enums;

namespace ModulbankInternship.Accounts.DTO;

public abstract class NewAccountRequest
{
    /// <summary>Тип счета</summary>
    public EAccountType AccountType { get; set; }
    
    /// <summary>Валюта счета</summary>
    public string Currency { get; set; }
    
    /// <summary>Процентная ставка по счету</summary>
    public decimal? InterestRate { get; set; }
}