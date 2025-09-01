using MediatR;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Accounts.Get;
using ModulbankInternship.Accounts.Get.ById;
using ModulbankInternship.Accounts.Transactions.Domain.Enums;
using ModulbankInternship.Transactions.Models;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Transfer.MakeTransfer;

public class TransferRequestHandler(IMediator mediator, IMakeTransferRepository transferRep)
    : IRequestHandler<MakeTransferCommand, bool>
{
    public async Task<bool> Handle(MakeTransferCommand request, CancellationToken cancellationToken)
    {
        var transferRequest = request.TransferRequest;
        var account = await mediator.Send(new GetAccountByIdInternalQuery(transferRequest.AccountId), cancellationToken);
        var counterpartyAccount = await mediator.Send(new GetAccountByIdInternalQuery(transferRequest.AccountId), cancellationToken);
        await mediator.Send(new CheckExecutorAccessCommand(account.OwnerId, request.Executor, new[] { EAccessClass.Owner }), cancellationToken);
        
        var creditTransaction = CreateTransferTransaction(account, transferRequest, ETransactionType.Credit);
        var debitTransaction = CreateTransferTransaction(counterpartyAccount, transferRequest, ETransactionType.Debit);
        await transferRep.MakeTransfer(debitTransaction, creditTransaction);

        return true;
    }
    
    private static TransactionModel CreateTransferTransaction(AccountModel account, TransferRequest request, ETransactionType type)
    {
        return new TransactionModel()
        {
            AccountId = account.Id,
            Amount = request.Amount,
            CounterpartyAccountId = request.CounterpartyAccountId,
            Currency = account.Currency,
            DateTime = DateTime.UtcNow,
            Description =
                $"{type.ToString()} transaction by transfer from Account id: {account.Id} to Account id: {request.CounterpartyAccountId}",
            Type = type
        };
    }
}