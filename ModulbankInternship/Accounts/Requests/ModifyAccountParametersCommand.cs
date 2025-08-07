using ModulbankInternship.Accounts.DTO;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Accounts.Requests;

public record ModifyAccountParametersCommand(Guid Id, ModifyAccountRequest ModifyAccountRequest, ExecutorData Executor)
    : ICommand<Dictionary<string, string>>;
