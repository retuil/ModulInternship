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
    public async Task<Guid> Handle(MakeTransactionCommand request, CancellationToken cancellationToken)
    {
        var newTransactionRequest = request.NewTransactionRequest;
        var ownerId = _AccountsRepository.Get(newTransactionRequest.AccountId).OwnerId;
        _mediator.Send(new CheckExecutorAccessCommand(ownerId, request.Executor,
            new [] { EAccessClass.Manager, EAccessClass.Cashier }), cancellationToken);
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
        
        var newTransactionId = await _mediator.Send(new NewTransactionCommand(newTransaction), cancellationToken);
        return newTransactionId;
    }
}