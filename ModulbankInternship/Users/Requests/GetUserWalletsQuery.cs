using MediatR;
using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Users.Requests;

public record GetUserAccountsQuery(Guid UserId, Guid ExecutorId) : IQuery<AccountModel[]>;