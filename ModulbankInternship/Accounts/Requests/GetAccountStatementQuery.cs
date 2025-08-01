using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Accounts.Requests;

public record GetAccountStatementQuery(Guid AccountId, DateTime StartDate, DateTime FinishDate, Guid ExecutorId)
    : IQuery<AccountStatementResponse>;