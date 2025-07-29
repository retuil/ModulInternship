using MediatR;
using ModulbankInternship.Account;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Wallets.Interfaces;
using ModulbankInternship.Wallets.Requests;

namespace ModulbankInternship.Wallets;

public class GetWalletByIdHandler(IMediator _mediator, IWalletsRepository _walletsRepository)
: IQueryHandler<GetWalletByIdQuery, WalletModel>
{
    public Task<WalletModel> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
    {
        var wallet = _walletsRepository.Get(request.WalletId);
        _mediator.Send(new CheckExecutorAccessCommand(wallet.OwnerId, request.ExecutorId,
            new[] { EAccessClass.Owner, EAccessClass.Manager, EAccessClass.Cashier }));

        return Task.FromResult(wallet);
    }
}