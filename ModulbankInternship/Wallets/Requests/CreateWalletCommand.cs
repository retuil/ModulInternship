using System.Windows.Input;
using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Wallets.Requests;

public record CreateWalletCommand(NewWalletRequest NewWalletRequest, Guid ExecutorId): ICommand<Guid>;