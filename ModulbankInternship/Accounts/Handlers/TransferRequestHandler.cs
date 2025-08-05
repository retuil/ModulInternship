using MediatR;
using ModulbankInternship.Account;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions;
using ModulbankInternship.Transactions.Enums;
using ModulbankInternship.Transactions.Models;
using ModulbankInternship.Transactions.Requests;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Requests;

namespace ModulbankInternship.Accounts;

public class TransferRequestHandler(IMediator _mediator, IAccountsRepository accountsRepository)
    : ICommandHandler<MakeTransferCommand, bool>
{
    public Task<bool> Handle(MakeTransferCommand request, CancellationToken cancellationToken)
    {
        var transferRequest = request.TransferRequest;
        var account = accountsRepository.Get(transferRequest.AccountId);
        var counterpartyAccount = accountsRepository.Get(transferRequest.AccountId);
        _mediator.Send(new CheckExecutorAccessCommand(account.OwnerId, request.Executor, new[] { EAccessClass.Owner }), cancellationToken);
        
        var creditTransaction = CreateTransferTransaction(account, transferRequest, ETransactionType.Credit);
        var debitTransaction = CreateTransferTransaction(counterpartyAccount, transferRequest, ETransactionType.Debit);
        _mediator.Send(new NewTransactionCommand(creditTransaction), cancellationToken);
        _mediator.Send(new NewTransactionCommand(debitTransaction), cancellationToken);

        return Task.FromResult(true);
    }
    
    private static TransactionModel CreateTransferTransaction(AccountModel account, TransferRequest request, ETransactionType type)
    {
        return new TransactionModel()
        {
            AccountId = account.Id,
            Amount = request.Amount,
            CounterpartyAccountId = request.CounterpartyAccountId,
            Currency = account.Currency,
            DateTime = DateTime.Now,
            Description =
                $"{type.ToString()} transaction by transfer from Account id: {account.Id} to Account id: {request.CounterpartyAccountId}",
            Type = type
        };
    }
}