using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Wallets.Requests;

public record ModifyWalletParametersCommand(Guid Id, ModifyWalletRequest ModifyWalletRequest, Guid ExecutorId)
    : ICommand<Dictionary<string, string>>;
