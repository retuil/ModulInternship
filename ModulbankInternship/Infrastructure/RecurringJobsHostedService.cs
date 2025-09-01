using Hangfire;
using ModulbankInternship.Accounts.Modify;

namespace ModulbankInternship.Infrastructure
{
    public class RecurringJobsHostedService(IServiceProvider serviceProvider) : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

            recurringJobManager.AddOrUpdate<IModifyAccountRepository>(
                "daily-interest-accrual",
                svc => svc.AccrueInterestsAsync(),
                Cron.Daily
            );

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}