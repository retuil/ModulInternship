using MediatR;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Models;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Users.Handlers;

public class GetOwnerAccountsHandler(IMediator mediator, IAccountsRepository accountsRepository)
    : IQueryHandler<GetOwnerAccountsQuery, AccountModel[]>
{
    public async Task<AccountModel[]> Handle(GetOwnerAccountsQuery request, CancellationToken cancellationToken)
    {
        await mediator.Send(new CheckExecutorAccessCommand(request.UserId, request.Executor,
            new[] { EAccessClass.Owner, }), cancellationToken);
        var accounts =  await accountsRepository.GetAllByOwnerIdAsync(request.UserId);
        return accounts.ToArray();
    }
}