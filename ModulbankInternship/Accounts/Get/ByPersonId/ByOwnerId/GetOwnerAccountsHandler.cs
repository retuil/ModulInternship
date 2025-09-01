using MediatR;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Accounts.Get.ByOwnerId;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Get.ByPersonId.ByOwnerId;

public class GetOwnerAccountsHandler(IMediator mediator, IGetAccountsByPersonIdRepository accountsRepository)
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