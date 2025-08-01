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

public class TransferRequestHandler(IMediator _mediator, IAccountsRepository _AccountsRepository)
    : ICommandHandler<MakeTransferCommand, bool>
{
    public Task<bool> Handle(MakeTransferCommand request, CancellationToken cancellationToken)
    {
        var transferRequest = request.TransferRequest;
        var Account = _AccountsRepository.Get(transferRequest.AccountId);
        var counterpartyAccount = _AccountsRepository.Get(transferRequest.AccountId);
        _mediator.Send(new CheckExecutorAccessCommand(Account.OwnerId, request.ExecutorId, new[] { EAccessClass.Owner }));
        
        var creditTransaction = CreateTransferTransaction(Account, transferRequest, ETransactionType.Credit);
        var debitTransaction = CreateTransferTransaction(counterpartyAccount, transferRequest, ETransactionType.Debit);
        _mediator.Send(new AddTransactionToAccountCommand(creditTransaction), cancellationToken);
        _mediator.Send(new AddTransactionToAccountCommand(debitTransaction), cancellationToken);
        _mediator.Send(new AddTransactionToRepositoryCommand(creditTransaction), cancellationToken);
        _mediator.Send(new AddTransactionToRepositoryCommand(debitTransaction), cancellationToken);

        return Task.FromResult(true);
    }
    
    private static TransactionModel CreateTransferTransaction(AccountModel Account, TransferRequest request, ETransactionType type)
    {
        return new TransactionModel()
        {
            AccountId = Account.Id,
            Amount = request.Amount,
            CounterpartyAccountId = request.CounterpartyAccountId,
            Currency = Account.Currency,
            DateTime = DateTime.Now,
            Description =
                $"{type.ToString()} transaction by transfer from Account id: {Account.Id} to Account id: {request.CounterpartyAccountId}",
            Type = type
        };
    }
}