using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Wallets.Requests;

public record GetWalletByIdQuery(Guid WalletId, Guid ExecutorId): IQuery<WalletModel>;