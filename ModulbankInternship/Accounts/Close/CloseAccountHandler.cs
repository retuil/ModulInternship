using MediatR;
using ModulbankInternship.Accounts.Get;
using ModulbankInternship.Accounts.Get.ById;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Close;

public class CloseAccountHandler(IMediator mediator, ICloseAccountRepository rep): IRequestHandler<CloseAccountCommand, bool>
{
    public async Task<bool> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await mediator.Send(new GetAccountByIdInternalQuery(request.AccountId), cancellationToken);
        
        await mediator.Send(new CheckExecutorAccessCommand(account.OwnerId, request.Executor,
                    new[] { EAccessClass.Owner, EAccessClass.Manager }), cancellationToken);
        
        await rep.DeleteAsync(request.AccountId);
        return true;
    }
}