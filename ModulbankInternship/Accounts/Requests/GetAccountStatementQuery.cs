using ModulbankInternship.Accounts.DTO;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Accounts.Requests;

public record GetAccountStatementQuery(Guid AccountId, DateTime StartDate, DateTime FinishDate, ExecutorData Executor)
    : IQuery<AccountStatementResponse>;