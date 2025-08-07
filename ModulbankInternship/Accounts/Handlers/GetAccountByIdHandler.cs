using MediatR;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Models;
using ModulbankInternship.Accounts.Requests;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Handlers;

public class GetAccountByIdHandler(IMediator mediator, IAccountsRepository accountsRepository)
: IQueryHandler<GetAccountByIdQuery, AccountModel>
{
    public Task<AccountModel> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = accountsRepository.Get(request.AccountId);
        mediator.Send(new CheckExecutorAccessCommand(account.OwnerId, request.Executor,
            new[] { EAccessClass.Owner, EAccessClass.Manager, EAccessClass.Cashier }), cancellationToken);

        return Task.FromResult(account);
    }
}