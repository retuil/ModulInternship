using ModulbankInternship.Accounts.Domain.Models;

namespace ModulbankInternship.Accounts.Get.ById;

public interface IGetAccountByIdRepository
{
    public Task<AccountModel?> GetByIdAsync(Guid id);
}