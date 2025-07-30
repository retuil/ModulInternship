using MediatR;
using ModulbankInternship.Account;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions;
using ModulbankInternship.Transactions.Enums;
using ModulbankInternship.Transactions.Models;
using ModulbankInternship.Transactions.Requests;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Wallets.Interfaces;
using ModulbankInternship.Wallets.Requests;

namespace ModulbankInternship.Wallets;

public class TransferRequestHandler(IMediator _mediator, IWalletsRepository _walletsRepository)
    : ICommandHandler<MakeTransferCommand, bool>
{
    public Task<bool> Handle(MakeTransferCommand request, CancellationToken cancellationToken)
    {
        var transferRequest = request.TransferRequest;
        var wallet = _walletsRepository.Get(transferRequest.WalletId);
        var counterpartyWallet = _walletsRepository.Get(transferRequest.WalletId);
        _mediator.Send(new CheckExecutorAccessCommand(wallet.OwnerId, request.ExecutorId, new[] { EAccessClass.Owner }));
        
        var creditTransaction = CreateTransferTransaction(wallet, transferRequest, ETransactionType.Credit);
        var debitTransaction = CreateTransferTransaction(counterpartyWallet, transferRequest, ETransactionType.Debit);
        _mediator.Send(new AddTransactionToWalletCommand(creditTransaction), cancellationToken);
        _mediator.Send(new AddTransactionToWalletCommand(debitTransaction), cancellationToken);
        _mediator.Send(new AddTransactionToRepositoryCommand(creditTransaction), cancellationToken);
        _mediator.Send(new AddTransactionToRepositoryCommand(debitTransaction), cancellationToken);

        return Task.FromResult(true);
    }
    
    private static TransactionModel CreateTransferTransaction(WalletModel wallet, TransferRequest request, ETransactionType type)
    {
        return new TransactionModel()
        {
            WalletId = wallet.Id,
            Amount = request.Amount,
            CounterpartyWalletId = request.CounterpartyWalletId,
            Currency = wallet.Currency,
            DateTime = DateTime.Now,
            Description =
                $"{type.ToString()} transaction by transfer from wallet id: {wallet.Id} to wallet id: {request.CounterpartyWalletId}",
            Type = type
        };
    }
}