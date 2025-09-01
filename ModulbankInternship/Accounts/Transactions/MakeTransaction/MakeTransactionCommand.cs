using ModulbankInternship.Accounts.Transactions.MakeTransaction;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Transactions.Requests;

public record MakeTransactionCommand(NewTransactionRequest NewTransactionRequest, ExecutorData Executor)
    : ICommand<Guid>;