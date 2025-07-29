namespace ModulbankInternship.Account;

public class NewWalletRequest
{
    public Guid OwnerId { get; set; }
    public EWalletType WalletType { get; set; }
    public string Currency { get; set; }
    public decimal? InterestRate { get; set; }
}