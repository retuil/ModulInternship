using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Accounts.Interfaces;

namespace ModulbankInternship.Accounts;

public class AccountsRepository: BaseRepository<AccountModel>, IAccountsRepository
{
    public AccountModel[] GetAllByOwner(Guid ownerId)
    {
        return dataBase.Where(w => w.OwnerId == ownerId).ToArray();
    }
}