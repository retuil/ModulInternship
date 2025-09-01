namespace ModulbankInternship.Accounts.Transfer;

public class TransferRequest
{
    /// <summary>Id счета, с которого инициируется перевод</summary>
    public Guid AccountId { get; set; }
    
    /// <summary>Id счета, на который инициируется перевод</summary>
    public Guid CounterpartyAccountId { get; set; }
    
    /// <summary>Сумма перевода</summary>
    public decimal Amount { get; set; }
}