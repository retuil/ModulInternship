using MediatR;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions.Models;
using ModulbankInternship.Transactions.Requests;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Requests;

namespace ModulbankInternship.Transactions.Handlers;

public class MakeTransactionHandler(IMediator _mediator, IAccountsRepository _AccountsRepository)
    : ICommandHandler<MakeTransactionCommand, Guid>
{
    public Task<Guid> Handle(MakeTransactionCommand request, CancellationToken cancellationToken)
    {
        var newTransactionRequest = request.NewTransactionRequest;
        var ownerId = _AccountsRepository.Get(newTransactionRequest.AccountId).OwnerId;
        _mediator.Send(new CheckExecutorAccessCommand(ownerId, request.ExecutorId,
            new [] { EAccessClass.Manager, EAccessClass.Cashier }));
        var newTransaction = new TransactionModel()
        {
            Amount = newTransactionRequest.Amount,
            CounterpartyAccountId = newTransactionRequest.CounterpartyAccountId,
            Currency = newTransactionRequest.Currency,
            DateTime = DateTime.Now,
            Description = newTransactionRequest.Description,
            Type = newTransactionRequest.TransactionType,
            AccountId = newTransactionRequest.AccountId
        };
        _mediator.Send(new AddTransactionToAccountCommand(newTransaction), cancellationToken);
        var newTransactionId = _mediator.Send(new AddTransactionToRepositoryCommand(newTransaction), cancellationToken);
        return newTransactionId;
    }
}