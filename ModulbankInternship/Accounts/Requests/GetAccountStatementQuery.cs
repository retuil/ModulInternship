using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.DTO;

namespace ModulbankInternship.Accounts.Requests;

public record GetAccountStatementQuery(Guid AccountId, DateTime StartDate, DateTime FinishDate, ExecutorData Executor)
    : IQuery<AccountStatementResponse>;