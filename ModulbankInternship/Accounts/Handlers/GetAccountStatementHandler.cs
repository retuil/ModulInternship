using MediatR;
using ModulbankInternship.Account;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Requests;

namespace ModulbankInternship.Accounts;

public class GetAccountStatementHandler(IMediator _mediator, IAccountsRepository _AccountsRepository)
    : IQueryHandler<GetAccountStatementQuery, AccountStatementResponse>
{
    public Task<AccountStatementResponse> Handle(GetAccountStatementQuery request, CancellationToken cancellationToken)
    {
        var startDate = request.StartDate;
        var finishDate = request.FinishDate;
        var account = _AccountsRepository.Get(request.AccountId);
        _mediator.Send(new CheckExecutorAccessCommand(account.OwnerId, request.Executor,
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