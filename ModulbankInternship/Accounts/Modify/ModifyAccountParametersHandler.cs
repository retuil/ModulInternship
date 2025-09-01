using MediatR;
using ModulbankInternship.Accounts.Get;
using ModulbankInternship.Accounts.Get.ById;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Modify;

public class ModifyAccountParametersHandler(IMediator mediator, IModifyAccountRepository rep)
    : IRequestHandler<ModifyAccountParametersCommand, Dictionary<string, string>>
{
    public async Task<Dictionary<string, string>> Handle(ModifyAccountParametersCommand request, CancellationToken cancellationToken)
    {
        var modifyAccountRequest = request.ModifyAccountRequest;
        var account = await mediator.Send(new GetAccountByIdInternalQuery(request.Id), cancellationToken);
        await mediator.Send(new CheckExecutorAccessCommand(account.OwnerId, request.Executor,
            new[] { EAccessClass.Manager }), cancellationToken);
        var changeReport = new Dictionary<string, string>();

        if (modifyAccountRequest.NewInterestRate is not null)
        {
            account.InterestRate = modifyAccountRequest.NewInterestRate;
            changeReport[nameof(account.InterestRate)] = modifyAccountRequest.NewInterestRate.ToString()!;
        }
        await rep.UpdateAsync(account);

        return changeReport;
    }
}