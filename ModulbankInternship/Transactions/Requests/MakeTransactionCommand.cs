using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions.DTO;

namespace ModulbankInternship.Transactions.Requests;

public record MakeTransactionCommand(NewTransactionRequest NewTransactionRequest, Guid ExecutorId)
    : ICommand<Guid>;