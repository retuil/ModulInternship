using Microsoft.EntityFrameworkCore;
using ModulbankInternship.Accounts.Domain.Enums;
using ModulbankInternship.Accounts.Domain.Exceptions;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Accounts.Get.ById;
using ModulbankInternship.Accounts.Transactions.Domain.Enums;
using ModulbankInternship.Accounts.Transactions.MakeTransaction;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Tests.IntegrationTests.ClientBlocked.Service;
using ModulbankInternship.Transactions.Requests;
using Tests.Helpers;

namespace ModulbankInternship.Tests.IntegrationTests.ClientBlocked;

public class ClientBlockedPreventsDebitTests
{
    private DbContextOptions<ApplicationDbContext> CreateNewInMemoryOptions(string dbName)
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
    }

    [Fact]
    public async Task ClientBlockedPreventsDebit_HandlerThrowsAndNoOutboxEntryCreated()
    {
        var dbName = Guid.NewGuid().ToString();
        var options = CreateNewInMemoryOptions(dbName);

        var accountId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();

        using (var seedCtx = new ApplicationDbContext(options))
        {
            await seedCtx.Database.EnsureCreatedAsync();

            var account = new AccountModel
            {
                Id = accountId,
                OwnerId = ownerId,
                Currency = "USD",
                AccountType = EAccountType.Checking,
                IsExist = true,
                IsFrozen = true 
            };

            seedCtx.Accounts.Add(account);
            await seedCtx.SaveChangesAsync();
        }

        using (var ctx = new ApplicationDbContext(options))
        {
            var mediator = new TestMediator(ctx);

            var getAccountRepo = new GetAccountByIdRepository(ctx);

            var handler = new MakeTransactionHandler(mediator, getAccountRepo);

            var newTxRequest = new NewTransactionRequest
            {
                AccountId = accountId,
                Amount = 100m,
                Currency = "USD",
                TransactionType = ETransactionType.Debit,
                Description = "integration test debit",
                CounterpartyAccountId = null
            };

            var executor = new ExecutorData
            {
                UserId = Guid.NewGuid(),
                Role = null
            };

            var command = new MakeTransactionCommand(newTxRequest, executor);

            await Assert.ThrowsAsync<AccountBlockedException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });

            var hasMoneyDebited = await ctx.OutboxMessages
                .AnyAsync(x => x.EventType == "MoneyDebited" || x.RoutingKey == "money.debited");
            Assert.False(hasMoneyDebited);
        }
    }
}