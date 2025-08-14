using Hangfire;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using ModulbankInternship.Accounts.Services;

namespace ModulbankInternship.Infrastructure
{
    public class RecurringJobsHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public RecurringJobsHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

            recurringJobManager.AddOrUpdate<InterestService>(
                "daily-interest-accrual",
                svc => svc.AccrueDailyInterestAsync(),
                Cron.Daily
            );

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}