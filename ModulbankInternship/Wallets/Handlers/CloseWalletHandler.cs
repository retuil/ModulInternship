using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Account;
using ModulbankInternship.Account.Exceptions;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Wallets.Interfaces;
using ModulbankInternship.Wallets.Requests;

namespace ModulbankInternship.Wallets;

public class CloseWalletHandler(IMediator _mediator, IWalletsRepository _walletsRepository)
    : ICommandHandler<CloseWalletCommand, bool>
{
    public Task<bool> Handle(CloseWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = _walletsRepository.Get(request.WalletId);
        _mediator.Send(new CheckExecutorAccessCommand(wallet.OwnerId, request.ExecutorId,
                    new[] { EAccessClass.Owner, EAccessClass.Manager }));
        if (!wallet.IsExist)
        {
            throw new ResourceNotFoundException($"No open wallet with id: {request.WalletId}");
        }

        _walletsRepository.Delete(request.WalletId);
        return Task.FromResult(true);
    }
}