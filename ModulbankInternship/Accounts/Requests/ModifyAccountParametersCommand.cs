using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.DTO;

namespace ModulbankInternship.Accounts.Requests;

public record ModifyAccountParametersCommand(Guid Id, ModifyAccountRequest ModifyAccountRequest, ExecutorData Executor)
    : ICommand<Dictionary<string, string>>;
