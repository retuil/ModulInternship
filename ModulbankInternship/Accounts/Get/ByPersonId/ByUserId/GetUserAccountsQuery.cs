using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Users.Requests;

public record GetUserAccountsQuery(Guid UserId, ExecutorData Executor) : IQuery<AccountModel[]>;