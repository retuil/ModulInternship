using ModulbankInternship.Accounts.DTO;

namespace ModulbankInternship.Accounts.Create.ForAnyUser;

public class NewAccountForAnyUserRequest: NewAccountRequest
{
    /// <summary>Id владельца счета</summary>
    public Guid OwnerId { get; set; }
}