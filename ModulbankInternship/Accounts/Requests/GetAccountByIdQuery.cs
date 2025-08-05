using ModulbankInternship.Accounts.Models;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Accounts.Requests;

public record GetAccountByIdQuery(Guid AccountId, ExecutorData Executor): IQuery<AccountModel>;