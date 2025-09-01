using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Transactions.MakeTransaction;
using ModulbankInternship.Transactions.Requests;

namespace ModulbankInternship.Accounts.Transactions.MakeTransaction;

public class AddTransactionToRepositoryHandler(IMakeTransactionRepository transactionsRep)
    : ICommandHandler<AddTransactionToRepositoryCommand, Guid>
{
    public async Task<Guid> Handle(AddTransactionToRepositoryCommand request, CancellationToken cancellationToken)
    {
        var transactionId = await transactionsRep.AddAsync(request.TransactionModel);
        return transactionId!.Value;
    }
}