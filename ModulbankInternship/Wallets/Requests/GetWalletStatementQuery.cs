using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Wallets.Requests;

public record GetWalletStatementQuery(Guid WalletId, DateTime StartDate, DateTime FinishDate, Guid ExecutorId)
    : IQuery<WalletStatementResponse>;