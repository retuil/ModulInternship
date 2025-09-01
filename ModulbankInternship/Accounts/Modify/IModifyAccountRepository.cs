using ModulbankInternship.Accounts.Domain.Models;

namespace ModulbankInternship.Accounts.Modify;

public interface IModifyAccountRepository
{
    public Task UpdateAsync(AccountModel account);

    public Task AccrueInterestsAsync();
}