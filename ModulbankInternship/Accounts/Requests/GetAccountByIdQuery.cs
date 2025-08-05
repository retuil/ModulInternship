using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.DTO;

namespace ModulbankInternship.Accounts.Requests;

public record GetAccountByIdQuery(Guid AccountId, ExecutorData Executor): IQuery<AccountModel>;