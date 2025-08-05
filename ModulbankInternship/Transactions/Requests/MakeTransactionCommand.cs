using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions.DTO;
using ModulbankInternship.Users.DTO;

namespace ModulbankInternship.Transactions.Requests;

public record MakeTransactionCommand(NewTransactionRequest NewTransactionRequest, ExecutorData Executor)
    : ICommand<Guid>;