using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Transactions.Requests;

public record AddTransactionToRepositoryCommand(TransactionModel TransactionModel): ICommand<Guid>;