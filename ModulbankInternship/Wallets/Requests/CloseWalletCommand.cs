using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Wallets.Requests;

public record CloseWalletCommand(Guid WalletId, Guid ExecutorId): ICommand<bool>;