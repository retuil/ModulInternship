using Microsoft.EntityFrameworkCore;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Accounts.Get.ById;

public class GetAccountByIdRepository(ApplicationDbContext context): IGetAccountByIdRepository
{
    public async Task<AccountModel?> GetByIdAsync(Guid id)
    {
        return await context.Accounts
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}