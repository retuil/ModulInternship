namespace ModulbankInternship.Accounts.Modify;

public class ModifyAccountRequest
{
    /// <summary>Новая процентная ставка по счету</summary>
    public decimal? NewInterestRate { get; set; }
}