using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModulbankInternship.Accounts.Domain.Enums;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Accounts.Get.ById;
using ModulbankInternship.Accounts.Transactions.MakeTransaction;
using ModulbankInternship.Accounts.Transfer;
using ModulbankInternship.Accounts.Transfer.MakeTransfer;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Transactions.MakeTransaction;
using Testcontainers.PostgreSql;

namespace ModulbankInternship.Tests.IntegrationTests.Balance
{
    public class ParallelTransferTests : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _pgContainer = new PostgreSqlBuilder()
            .WithDatabase("test_db")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithCleanUp(true)
            .Build();

        public async Task InitializeAsync()
        {
            await _pgContainer.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _pgContainer.StopAsync();
            await _pgContainer.DisposeAsync();
        }

        [Fact]
        public async Task ParallelTransfers_50Parallel_MaintainsTotalBalance()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(_pgContainer.GetConnectionString(),
                    npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()));

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MakeTransferCommand).Assembly));

            services.AddScoped<IGetAccountByIdRepository, GetAccountByIdRepository>();
            services.AddScoped<IMakeTransferRepository, MakeTransferRepository>();
            services.AddScoped<IMakeTransactionRepository, MakeTransactionRepository>();
            services.AddLogging();

            var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });

            var userId = Guid.NewGuid();

            using (var scope = provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await db.Database.EnsureCreatedAsync();
                await db.Accounts.AnyAsync();

                const int accountsCount = 5;
                const decimal initialBalancePerAccount = 1000m;

                if (!await db.Accounts.AnyAsync())
                {
                    var accounts = new List<AccountModel>();
                    for (var i = 0; i < accountsCount; i++)
                    {
                        accounts.Add(new AccountModel
                        {
                            Id = Guid.NewGuid(),
                            Balance = initialBalancePerAccount,
                            Currency = "RUB",
                            AccountType = EAccountType.Checking,
                            OwnerId = userId
                        });
                    }

                    db.Accounts.AddRange(accounts);
                    await db.SaveChangesAsync();
                }
            }

            decimal initialTotal;
            using (var scope = provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                initialTotal = await db.Accounts.SumAsync(a => a.Balance);
            }

            List<Guid> accountIds;
            using (var scope = provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                accountIds = await db.Accounts.Select(a => a.Id).ToListAsync();
            }

            const int parallelTransfersCount = 50;
            const decimal transferAmount = 10m;

            var rnd = new ThreadLocal<Random>(() => new Random());
            var tasks = new List<Task>();

            for (var i = 0; i < parallelTransfersCount; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    using var scope = provider.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    Guid from, to;
                    do
                    {
                        var idx1 = rnd.Value.Next(accountIds.Count);
                        var idx2 = rnd.Value.Next(accountIds.Count);
                        from = accountIds[idx1];
                        to = accountIds[idx2];
                    } while (from == to);

                    var transferRequest = new TransferRequest
                    {
                        AccountId = from,
                        CounterpartyAccountId = to,
                        Amount = transferAmount
                    };

                    var executor = new ExecutorData
                    {
                        UserId = userId,
                        Role = "Client"
                    };

                    var cmd = new MakeTransferCommand(transferRequest, executor);

                    var result = await mediator.Send(cmd);

                    if (!result)
                        throw new Exception("Transfer command returned false");
                }));
            }

            await Task.WhenAll(tasks);

            decimal finalTotal;
            using (var scope = provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                finalTotal = await db.Accounts.SumAsync(a => a.Balance);
            }

            Assert.Equal(initialTotal, finalTotal);
        }
    }
}
