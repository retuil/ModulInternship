using MediatR;
using ModulbankInternship.Accounts.Create;
using ModulbankInternship.Accounts.Create.ForAnyUser;
using ModulbankInternship.Accounts.Domain.Enums;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Accounts.Get;
using ModulbankInternship.Accounts.Get.ById;
using ModulbankInternship.Accounts.Statement;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Transactions.Models;
using ModulbankInternship.Users.Requests;
using Moq;

namespace ModulbankInternship.Tests.UnitTests;

public class AccountTests
{
    private readonly Mock<IMediator>? mediator;
    
    public AccountTests()
    {
        mediator = new Mock<IMediator>();
        mediator
            .Setup(m => m.Send(It.IsAny<CheckExecutorAccessCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
    }
    
    [Fact]
    public async Task TestStatementHandler()
    {
        var accountId = Guid.NewGuid();
        var executor = new ExecutorData()
        {
            UserId = Guid.NewGuid(),
            Role = "Manager"
        };
        var startDate = new DateTime(2025, 1, 1);
        var finishDate = new DateTime(2025, 1, 31);

        var transactions = new List<TransactionModel>
        {
            new TransactionModel() { DateTime = new DateTime(2025, 1, 5), Amount = 100 },
            new TransactionModel() { DateTime = new DateTime(2025, 2, 1), Amount = 200 }
        };

        var account = new AccountModel()
        {
            Id = accountId,
            OwnerId = Guid.NewGuid(),
            Transactions = transactions,
            IsExist = true
        };

        mediator
            .Setup(m => m.Send(It.IsAny<GetAccountByIdInternalQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        var handler = new GetAccountStatementHandler(mediator.Object);

        var query = new GetAccountStatementQuery(accountId, startDate, finishDate, executor);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Equal(startDate, result.StartDate);
        Assert.Equal(finishDate, result.FinishDate);
        Assert.Single(result.Transactions);
        Assert.Equal(100, result.Transactions.First().Amount);

        mediator.Verify(m => m.Send(It.IsAny<GetAccountByIdInternalQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        mediator.Verify(m => m.Send(It.IsAny<CheckExecutorAccessCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task TestCreateAccountForAnyUserHandler()
    {
        var accountsRepoMock = new Mock<ICreateAccountRepository>();

        var expectedAccountId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();

        var newAccountRequest = new NewAccountForAnyUserRequest()
        {
            Currency = "USD",
            InterestRate = 3.5m,
            OwnerId = ownerId,
            AccountType = EAccountType.Deposit
        };

        accountsRepoMock
            .Setup(r => r.AddAsync(It.IsAny<AccountModel>()))
            .ReturnsAsync(expectedAccountId);

        var handler = new CreateAccountHandler(mediator.Object, accountsRepoMock.Object);

        var executor = new ExecutorData
        {
            UserId = Guid.NewGuid(),
            Role = "Manager"
        };

        var command = new CreateAccountCommand(newAccountRequest, executor);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(expectedAccountId, result);

        mediator.Verify(m => m.Send(It.Is<CheckExecutorAccessCommand>(
            cmd => cmd.OwnerId == ownerId &&
                   cmd.Executor == executor), It.IsAny<CancellationToken>()), Times.Once);

        accountsRepoMock.Verify(r => r.AddAsync(It.Is<AccountModel>(
            a => a.OwnerId == ownerId &&
                 a.Currency == "USD" &&
                 a.InterestRate == 3.5m &&
                 a.AccountType == EAccountType.Deposit
        )), Times.Once);
    }

}
