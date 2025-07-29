using MediatR;
using ModulbankInternship.Account;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Wallets.Interfaces;
using ModulbankInternship.Wallets.Requests;

namespace ModulbankInternship.Wallets;

public class CreateWalletHandler(IMediator _mediator, IWalletsRepository _walletsRepository)
    : IRequestHandler<CreateWalletCommand, Guid>
{
    public Task<Guid> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        var newWalletRequest = request.NewWalletRequest;
        _mediator.Send(new CheckExecutorAccessCommand(newWalletRequest.OwnerId, request.ExecutorId,
            new[] { EAccessClass.Owner, EAccessClass.Manager }));

        var newWalletId = _walletsRepository.Add(new WalletModel()
        {
            Currency = newWalletRequest.Currency,
            InterestRate = newWalletRequest.InterestRate,
            OwnerId = newWalletRequest.OwnerId,
            WalletType = newWalletRequest.WalletType,
            OpeningDate = DateTime.Now
        });
        return Task.FromResult(newWalletId);
    }
}