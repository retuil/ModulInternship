using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Models;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Accounts.Repositories;

public class AccountsRepository(ApplicationDbContext context) : IAccountsRepository
{
    public async Task<AccountModel?> GetByIdAsync(Guid id)
    {
        return await context.Accounts
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<AccountModel>> GetAllByOwnerIdAsync(Guid ownerId)
    {
        return await context.Accounts
            .Include(a => a.Transactions)
            .Where(a => a.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<Guid?> AddAsync(AccountModel account)
    {
        context.Accounts.Add(account);
        await context.SaveChangesAsync();
        return account.Id;
    }

    public async Task UpdateAsync(AccountModel account)
    {
        context.Accounts.Update(account);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var account = await context.Accounts.FindAsync(id);
        if (account != null)
        {
            account.IsExist = false;
            context.Accounts.Update(account);
            await context.SaveChangesAsync();
        }
    }
}