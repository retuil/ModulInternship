using MediatR;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Accounts.Get.ByOwnerId;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Get.ByPersonId.ByUserId;

public class GetUserAccountsHandler(IMediator mediator, IGetAccountsByPersonIdRepository accountsRepository)
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