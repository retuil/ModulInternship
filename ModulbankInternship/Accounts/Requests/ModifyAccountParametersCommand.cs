using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Accounts.Requests;

public record ModifyAccountParametersCommand(Guid Id, ModifyAccountRequest ModifyAccountRequest, Guid ExecutorId)
    : ICommand<Dictionary<string, string>>;
