using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Accounts.Close;

public class CloseAccountRepository(ApplicationDbContext context): ICloseAccountRepository
{
    public async Task DeleteAsync(Guid id)
    {
        var account = await context.Accounts.FindAsync(id);
        if (account is not null)
        {
            account.IsExist = false;
            context.Accounts.Update(account);
            await context.SaveChangesAsync();
        }
    }

    public async Task BlockAccountAsync(Guid id)
    {
        var account = await context.Accounts.FindAsync(id);
        if (account is not null)
        {
            account.IsFrozen = true;
            context.Accounts.Update(account);
            await context.SaveChangesAsync();
        }
    }

    public async Task UnblockAccountAsync(Guid id)
    {
        var account = await context.Accounts.FindAsync(id);
        if (account is not null)
        {
            account.IsFrozen = false;
            context.Accounts.Update(account);
            await context.SaveChangesAsync();
        }
    }
}