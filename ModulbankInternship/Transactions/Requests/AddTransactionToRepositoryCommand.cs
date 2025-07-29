using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Transactions.Requests;

public record AddTransactionToRepositoryCommand(TransactionModel TransactionModel): ICommand<Guid>;