using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions.Requests;
using ModulbankInternship.Wallets.Interfaces;

namespace ModulbankInternship.Transactions.Handlers;

public class AddTransactionToRepositoryHandler(ITransactionsRepository _transactionsRepository)
    : ICommandHandler<AddTransactionToRepositoryCommand, Guid>
{
    public Task<Guid> Handle(AddTransactionToRepositoryCommand request, CancellationToken cancellationToken)
    {
        var transactionId = _transactionsRepository.Add(request.TransactionModel);
        return Task.FromResult(transactionId);
    }
}