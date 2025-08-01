using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Accounts.Requests;

public record GetAccountByIdQuery(Guid AccountId, Guid ExecutorId): IQuery<AccountModel>;