using MediatR;
using ModulbankInternship.Accounts.Get;
using ModulbankInternship.Accounts.Get.ById;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Statement;

public class GetAccountStatementHandler(IMediator mediator): IRequestHandler<GetAccountStatementQuery, AccountStatementResponse>
{
    public async Task<AccountStatementResponse> Handle(GetAccountStatementQuery request, CancellationToken cancellationToken)
    {
        var startDate = request.StartDate;
        var finishDate = request.FinishDate;
        var account = await mediator.Send(new GetAccountByIdInternalQuery(request.AccountId), cancellationToken);
        
        await mediator.Send(new CheckExecutorAccessCommand(account.OwnerId, request.Executor,
            new[] { EAccessClass.Owner, EAccessClass.Manager }), cancellationToken);
        
        var statementTransactions = account.Transactions
            .Where(t => t.DateTime >= startDate && t.DateTime <= finishDate);
        var response = new AccountStatementResponse()
        {
            CreationDate = DateTime.UtcNow,
            StartDate = startDate,
            FinishDate = finishDate,
            Transactions = statementTransactions
        };

        return response;
    }
}