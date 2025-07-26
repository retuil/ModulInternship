namespace ModulbankInternship.Account;

public class TransferRequest
{
    public Guid WalletId { get; set; }
    public Guid CounterpartyWalletId { get; set; }
    public decimal Amount { get; set; }
    public string Message { get; set; }
}