namespace ModulbankInternship.Accounts.DTO;

public class NewAccountForAnyUserRequest: NewAccountRequest
{
    /// <summary>Id владельца счета</summary>
    public Guid OwnerId { get; set; }
}