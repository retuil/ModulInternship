using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Accounts.Get;

public record GetAccountByIdQuery(Guid AccountId, ExecutorData Executor): IQuery<AccountModel>;