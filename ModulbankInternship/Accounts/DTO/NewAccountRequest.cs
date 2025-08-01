namespace ModulbankInternship.Account;

public class NewAccountRequest
{
    /// <summary>Id владельца счета</summary>
    public Guid OwnerId { get; set; }
    
    /// <summary>Тип счета</summary>
    public EAccountType AccountType { get; set; }
    
    /// <summary>Валюта счета</summary>
    public string Currency { get; set; }
    
    /// <summary>Процентная ставка по счету</summary>
    public decimal? InterestRate { get; set; }
}