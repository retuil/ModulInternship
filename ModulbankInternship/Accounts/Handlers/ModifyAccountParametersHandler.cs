using MediatR;
using ModulbankInternship.Accounts.Exceptions;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Requests;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Handlers;

public class ModifyAccountParametersHandler(IMediator mediator, IAccountsRepository accountsRepository)
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
            changeReport[nameof(account.InterestRate)] = modifyAccountRequest.NewInterestRate.ToString();
        }
        await accountsRepository.UpdateAsync(account);

        return changeReport;
    }
}