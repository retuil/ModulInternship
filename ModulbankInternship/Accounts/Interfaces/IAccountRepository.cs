using ModulbankInternship.Accounts.Models;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Accounts.Interfaces;

public interface IAccountsRepository: IRepository<AccountModel>
{
    public AccountModel[] GetAllByOwner(Guid ownerId);
}