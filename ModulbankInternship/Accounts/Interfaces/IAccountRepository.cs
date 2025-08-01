using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Accounts.Interfaces;

public interface IAccountsRepository: IRepository<AccountModel>
{
    public AccountModel[] GetAllByOwner(Guid ownerId);
}