using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions.Requests;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Transactions.Interfaces;

namespace ModulbankInternship.Transactions.Handlers;

public class AddTransactionToRepositoryHandler(ITransactionsRepository _transactionsRepository)
    : ICommandHandler<AddTransactionToRepositoryCommand, Guid>
{
    public async Task<Guid> Handle(AddTransactionToRepositoryCommand request, CancellationToken cancellationToken)
    {
        var transactionId = await _transactionsRepository.AddAsync(request.TransactionModel);
        return transactionId.Value;
    }
}