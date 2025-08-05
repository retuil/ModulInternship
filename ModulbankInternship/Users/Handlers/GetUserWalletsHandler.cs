using MediatR;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Models;
using ModulbankInternship.Users.Enums;


namespace ModulbankInternship.Users.Handlers;

public class GetUserAccountsHandler(IMediator mediator, IAccountsRepository accountsRepository)
    : IRequestHandler<GetUserAccountsQuery, AccountModel[]>
{
    public Task<AccountModel[]> Handle(GetUserAccountsQuery request, CancellationToken cancellationToken)
    {
        mediator.Send(new CheckExecutorAccessCommand(request.UserId, request.Executor,
            new[] { EAccessClass.Manager }), cancellationToken);
        var accounts = accountsRepository.GetAllByOwner(request.UserId).ToArray();
        return Task.FromResult(accounts);
    }
}