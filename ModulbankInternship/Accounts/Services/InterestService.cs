using Microsoft.EntityFrameworkCore;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Accounts.Services;

public class InterestService(ApplicationDbContext context)
{
    public async Task AccrueDailyInterestAsync()
    {
        var accounts = await context.Accounts
            .Where(a => a.Balance > 0 && a.InterestRate.HasValue && a.InterestRate > 0 && a.IsExist)
            .ToListAsync();

        foreach (var account in accounts)
        {
            var interestRate = account.InterestRate.Value;
            var interest = account.Balance * interestRate;
            account.Balance += interest;
        }

        await context.SaveChangesAsync();
    }
}
