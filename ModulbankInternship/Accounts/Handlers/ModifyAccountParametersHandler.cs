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
        var Account = _AccountsRepository.Get(request.Id);
        _mediator.Send(new CheckExecutorAccessCommand(Account.OwnerId, request.ExecutorId,
            new[] { EAccessClass.Manager }));
        var changeReport = new Dictionary<string, string>();

        if (modifyAccountRequest.NewInterestRate is not null)
        {
            Account.InterestRate = modifyAccountRequest.NewInterestRate;
            changeReport[nameof(Account.InterestRate)] = Account.InterestRate.ToString();
        }
        _AccountsRepository.Update(Account);

        return Task.FromResult(changeReport);
    }
}