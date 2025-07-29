using MediatR;
using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Users.Requests;

public record GetUserWalletsQuery(Guid UserId, Guid ExecutorId) : IQuery<WalletModel[]>;