using MediatR;
using ModulbankInternship.Account;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Requests;

namespace ModulbankInternship.Accounts;

public class GetAccountByIdHandler(IMediator _mediator, IAccountsRepository _AccountsRepository)
: IQueryHandler<GetAccountByIdQuery, AccountModel>
{
    public Task<AccountModel> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = _AccountsRepository.Get(request.AccountId);
        _mediator.Send(new CheckExecutorAccessCommand(account.OwnerId, request.Executor,
            new[] { EAccessClass.Owner, EAccessClass.Manager, EAccessClass.Cashier }), cancellationToken);

        return Task.FromResult(account);
    }
}