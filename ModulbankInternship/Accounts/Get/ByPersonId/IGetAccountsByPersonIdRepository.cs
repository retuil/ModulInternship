using ModulbankInternship.Accounts.Domain.Models;

namespace ModulbankInternship.Accounts.Get.ByOwnerId;

public interface IGetAccountsByPersonIdRepository
{
    public Task<IEnumerable<AccountModel>> GetAllByOwnerIdAsync(Guid ownerId);
}