using MediatR;
using ModulbankInternship.Accounts.Exceptions;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Models;
using ModulbankInternship.Accounts.Requests;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Handlers;

public class GetAccountByIdHandler(IMediator mediator, IAccountsRepository accountsRepository)
{
    public async Task<AccountModel> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await mediator.Send(new GetAccountByIdInternalQuery(request.AccountId), cancellationToken);
        
        await mediator.Send(new CheckExecutorAccessCommand(account.OwnerId, request.Executor,
            new[] { EAccessClass.Owner, EAccessClass.Manager, EAccessClass.Cashier }), cancellationToken);

        return account;
    }
}