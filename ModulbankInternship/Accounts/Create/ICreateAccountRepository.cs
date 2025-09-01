using ModulbankInternship.Accounts.Domain.Models;

namespace ModulbankInternship.Accounts.Create;

public interface ICreateAccountRepository
{
    public Task<Guid?> AddAsync(AccountModel account);
}