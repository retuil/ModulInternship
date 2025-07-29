using MediatR;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions.Requests;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Wallets.Interfaces;
using ModulbankInternship.Wallets.Requests;

namespace ModulbankInternship.Transactions.Handlers;

public class MakeTransactionHandler(IMediator _mediator, IWalletsRepository _walletsRepository)
    : ICommandHandler<MakeTransactionCommand, Guid>
{
    public Task<Guid> Handle(MakeTransactionCommand request, CancellationToken cancellationToken)
    {
        var newTransactionRequest = request.NewTransactionRequest;
        var ownerId = _walletsRepository.Get(newTransactionRequest.walletId).OwnerId;
        _mediator.Send(new CheckExecutorAccessCommand(ownerId, request.ExecutorId,
            new [] { EAccessClass.Manager, EAccessClass.Cashier }));
        var newTransaction = new TransactionModel()
        {
            Amount = newTransactionRequest.Amount,
            CounterpartyWalletId = newTransactionRequest.counterpartyAccountId,
            Currency = newTransactionRequest.Currency,
            DateTime = DateTime.Now,
            Description = newTransactionRequest.Description,
            Type = newTransactionRequest.TransactionType,
            WalletId = newTransactionRequest.walletId
        };
        _mediator.Send(new AddTransactionToWalletCommand(newTransaction), cancellationToken);
        var newTransactionId = _mediator.Send(new AddTransactionToRepositoryCommand(newTransaction), cancellationToken);
        return newTransactionId;
    }
}