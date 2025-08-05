using MediatR;
using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.DTO;

namespace ModulbankInternship.Users.Requests;

public record GetUserAccountsQuery(Guid UserId, ExecutorData Executor) : IQuery<AccountModel[]>;