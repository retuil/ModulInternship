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
        var Account = _AccountsRepository.Get(request.AccountId);
        _mediator.Send(new CheckExecutorAccessCommand(Account.OwnerId, request.ExecutorId,
            new[] { EAccessClass.Owner, EAccessClass.Manager, EAccessClass.Cashier }));

        return Task.FromResult(Account);
    }
}