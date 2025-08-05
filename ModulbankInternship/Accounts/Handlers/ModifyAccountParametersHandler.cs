using MediatR;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Requests;

namespace ModulbankInternship.Accounts;

public class ModifyAccountParametersHandler(IMediator _mediator, IAccountsRepository _AccountsRepository)
    : ICommandHandler<ModifyAccountParametersCommand, Dictionary<string, string>>
{
    public Task<Dictionary<string, string>> Handle(ModifyAccountParametersCommand request, CancellationToken cancellationToken)
    {
        var modifyAccountRequest = request.ModifyAccountRequest;
        var account = _AccountsRepository.Get(request.Id);
        _mediator.Send(new CheckExecutorAccessCommand(account.OwnerId, request.Executor,
            new[] { EAccessClass.Manager }), cancellationToken);
        var changeReport = new Dictionary<string, string>();

        if (modifyAccountRequest.NewInterestRate is not null)
        {
            account.InterestRate = modifyAccountRequest.NewInterestRate;
            changeReport[nameof(account.InterestRate)] = account.InterestRate.ToString();
        }
        _AccountsRepository.Update(account);

        return Task.FromResult(changeReport);
    }
}