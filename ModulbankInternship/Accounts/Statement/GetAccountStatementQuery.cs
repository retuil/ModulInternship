using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Accounts.Statement;

public record GetAccountStatementQuery(Guid AccountId, DateTime StartDate, DateTime FinishDate, ExecutorData Executor)
    : IQuery<AccountStatementResponse>;