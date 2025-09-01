using Microsoft.EntityFrameworkCore;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Accounts.Get.ByOwnerId;

public class GetAccountsByPersonIdRepository(ApplicationDbContext context): IGetAccountsByPersonIdRepository
{
    public async Task<IEnumerable<AccountModel>> GetAllByOwnerIdAsync(Guid ownerId)
    {
        return await context.Accounts
            .Include(a => a.Transactions)
            .Where(a => a.OwnerId == ownerId)
            .ToListAsync();
    }
}