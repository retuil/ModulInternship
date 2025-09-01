using Microsoft.EntityFrameworkCore;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Accounts.AntifraudBlocks;

public class BlockAccountRepository(ApplicationDbContext context): IBlockAccountRepository
{
    public async Task BlockAccountAsync(Guid id)
    {
        var account = await context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        account!.IsFrozen = true;
        context.Accounts.Update(account);
        await context.SaveChangesAsync();
    }

    public async Task UnblockAccountAsync(Guid id)
    {
        var account = await context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        account!.IsFrozen = false;
        context.Accounts.Update(account);
        await context.SaveChangesAsync();
    }
}