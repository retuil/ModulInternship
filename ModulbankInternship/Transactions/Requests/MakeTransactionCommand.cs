using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Transactions.DTO;

namespace ModulbankInternship.Transactions.Requests;

public record MakeTransactionCommand(NewTransactionRequest NewTransactionRequest, ExecutorData Executor)
    : ICommand<Guid>;