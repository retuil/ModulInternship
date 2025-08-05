using MediatR;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Requests;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Handlers;

public class ModifyAccountParametersHandler(IMediator mediator, IAccountsRepository accountsRepository)
    : ICommandHandler<ModifyAccountParametersCommand, Dictionary<string, string>>
{
    public Task<Dictionary<string, string>> Handle(ModifyAccountParametersCommand request, CancellationToken cancellationToken)
    {
        var modifyAccountRequest = request.ModifyAccountRequest;
        var account = accountsRepository.Get(request.Id);
        mediator.Send(new CheckExecutorAccessCommand(account.OwnerId, request.Executor,
            new[] { EAccessClass.Manager }), cancellationToken);
        var changeReport = new Dictionary<string, string>();

        if (modifyAccountRequest.NewInterestRate is not null)
        {
            account.InterestRate = modifyAccountRequest.NewInterestRate;
            changeReport[nameof(account.InterestRate)] = account.InterestRate.ToString();
        }
        accountsRepository.Update(account);

        return Task.FromResult(changeReport);
    }
}