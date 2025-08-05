using MediatR;
using ModulbankInternship.Accounts.DTO;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Models;
using ModulbankInternship.Accounts.Requests;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Transactions.Enums;
using ModulbankInternship.Transactions.Models;
using ModulbankInternship.Transactions.Requests;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Handlers;

public class TransferRequestHandler(IMediator mediator, IAccountsRepository accountsRepository)
    : ICommandHandler<MakeTransferCommand, bool>
{
    public Task<bool> Handle(MakeTransferCommand request, CancellationToken cancellationToken)
    {
        var transferRequest = request.TransferRequest;
        var account = accountsRepository.Get(transferRequest.AccountId);
        var counterpartyAccount = accountsRepository.Get(transferRequest.AccountId);
        mediator.Send(new CheckExecutorAccessCommand(account.OwnerId, request.Executor, new[] { EAccessClass.Owner }), cancellationToken);
        
        var creditTransaction = CreateTransferTransaction(account, transferRequest, ETransactionType.Credit);
        var debitTransaction = CreateTransferTransaction(counterpartyAccount, transferRequest, ETransactionType.Debit);
        mediator.Send(new NewTransactionCommand(creditTransaction), cancellationToken);
        mediator.Send(new NewTransactionCommand(debitTransaction), cancellationToken);

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