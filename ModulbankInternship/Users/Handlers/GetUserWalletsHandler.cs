using MediatR;
using ModulbankInternship.Account;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Users;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Accounts;
using ModulbankInternship.Accounts.Interfaces;


namespace ModulbankInternship.Users.Handlers;

public class GetUserAccountsHandler(IMediator _mediator, IAccountsRepository AccountsRepository)
    : IRequestHandler<GetUserAccountsQuery, AccountModel[]>
{
    public Task<AccountModel[]> Handle(GetUserAccountsQuery request, CancellationToken cancellationToken)
    {
        _mediator.Send(new CheckExecutorAccessCommand(request.UserId, request.Executor,
            new[] { EAccessClass.Manager }), cancellationToken);
        var accounts = AccountsRepository.GetAllByOwner(request.UserId).ToArray();
        return Task.FromResult(accounts);
    }
}