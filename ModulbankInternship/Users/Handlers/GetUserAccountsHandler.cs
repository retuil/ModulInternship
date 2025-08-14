using MediatR;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Models;
using ModulbankInternship.Users.Enums;


namespace ModulbankInternship.Users.Handlers;

public class GetUserAccountsHandler(IMediator mediator, IAccountsRepository accountsRepository)
    : IRequestHandler<GetUserAccountsQuery, AccountModel[]>
{
    public async Task<AccountModel[]> Handle(GetUserAccountsQuery request, CancellationToken cancellationToken)
    {
        await mediator.Send(new CheckExecutorAccessCommand(request.UserId, request.Executor,
            new[] { EAccessClass.Manager, }), cancellationToken);
        var accounts =  await accountsRepository.GetAllByOwnerIdAsync(request.UserId);
        return accounts.ToArray();
    }
}