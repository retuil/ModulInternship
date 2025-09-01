using MediatR;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Get.ById;

public class GetAccountByIdHandler(IMediator mediator): IRequestHandler<GetAccountByIdQuery, AccountModel>
{
    public async Task<AccountModel> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await mediator.Send(new GetAccountByIdInternalQuery(request.AccountId), cancellationToken);
        
        await mediator.Send(new CheckExecutorAccessCommand(account.OwnerId, request.Executor,
            new[] { EAccessClass.Owner, EAccessClass.Manager, EAccessClass.Cashier }), cancellationToken);

        return account;
    }
}