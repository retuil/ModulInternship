using MediatR;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Wallets.Interfaces;
using ModulbankInternship.Wallets.Requests;

namespace ModulbankInternship.Wallets;

public class ModifyWalletParametersHandler(IMediator _mediator, IWalletsRepository _walletsRepository)
    : ICommandHandler<ModifyWalletParametersCommand, Dictionary<string, string>>
{
    public Task<Dictionary<string, string>> Handle(ModifyWalletParametersCommand request, CancellationToken cancellationToken)
    {
        var modifyWalletRequest = request.ModifyWalletRequest;
        var wallet = _walletsRepository.Get(request.Id);
        _mediator.Send(new CheckExecutorAccessCommand(wallet.OwnerId, request.ExecutorId,
            new[] { EAccessClass.Manager }));
        var changeReport = new Dictionary<string, string>();

        if (modifyWalletRequest.NewInterestRate is not null)
        {
            wallet.InterestRate = modifyWalletRequest.NewInterestRate;
            changeReport[nameof(wallet.InterestRate)] = wallet.InterestRate.ToString();
        }
        _walletsRepository.Update(wallet);

        return Task.FromResult(changeReport);
    }
}