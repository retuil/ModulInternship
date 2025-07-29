using MediatR;
using ModulbankInternship.Account;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Users;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Wallets;
using ModulbankInternship.Wallets.Interfaces;


namespace ModulbankInternship.Users.Handlers;

public class GetUserWalletsHandler(IMediator _mediator, IWalletsRepository walletsRepository)
    : IRequestHandler<GetUserWalletsQuery, WalletModel[]>
{
    public Task<WalletModel[]> Handle(GetUserWalletsQuery request, CancellationToken cancellationToken)
    {
        _mediator.Send(new CheckExecutorAccessCommand(request.UserId, request.ExecutorId,
            new[] { EAccessClass.Manager }));
        var wallets = walletsRepository.GetAllByOwner(request.UserId).ToArray();
        return Task.FromResult(wallets);
    }
}