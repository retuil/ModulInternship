using MediatR;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions.Models;
using ModulbankInternship.Transactions.Requests;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Requests;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Users.Enums;

namespace ModulbankInternship.Transactions.Handlers;

public class MakeTransactionHandler(IMediator mediator, IAccountsRepository accountsRepository)
    : ICommandHandler<MakeTransactionCommand, Guid>
{
    public async Task<Guid> Handle(MakeTransactionCommand request, CancellationToken cancellationToken)
    {
        var newTransactionRequest = request.NewTransactionRequest;
        var accountModel = await accountsRepository.GetByIdAsync(newTransactionRequest.AccountId);
        var ownerId = accountModel.OwnerId;
        await mediator.Send(new CheckExecutorAccessCommand(ownerId, request.Executor,
            new [] { EAccessClass.Manager, EAccessClass.Cashier }), cancellationToken);
        var newTransaction = new TransactionModel()
        {
            Amount = newTransactionRequest.Amount,
            CounterpartyAccountId = newTransactionRequest.CounterpartyAccountId,
            Currency = newTransactionRequest.Currency,
            DateTime = DateTime.UtcNow,
            Description = newTransactionRequest.Description,
            Type = newTransactionRequest.TransactionType,
            AccountId = newTransactionRequest.AccountId
        };
        
        var newTransactionId = await mediator.Send(new AddTransactionToRepositoryCommand(newTransaction), cancellationToken);
        return newTransactionId;
    }
}