using MediatR;
using ModulbankInternship.Accounts.DTO;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Requests;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Handlers;

public class GetAccountStatementHandler(IMediator mediator, IAccountsRepository accountsRepository)
    : IQueryHandler<GetAccountStatementQuery, AccountStatementResponse>
{
    public Task<AccountStatementResponse> Handle(GetAccountStatementQuery request, CancellationToken cancellationToken)
    {
        var startDate = request.StartDate;
        var finishDate = request.FinishDate;
        var account = accountsRepository.Get(request.AccountId);
        mediator.Send(new CheckExecutorAccessCommand(account.OwnerId, request.Executor,
            new[] { EAccessClass.Owner, EAccessClass.Manager }), cancellationToken);
        
        var statementTransactions = account.Transactions
            .Where(t => t.DateTime >= startDate && t.DateTime <= finishDate);
        var response = new AccountStatementResponse()
        {
            CreationDate = DateTime.Now,
            StartDate = startDate,
            FinishDate = finishDate,
            Transactions = statementTransactions
        };

        return Task.FromResult(response);
    }
}