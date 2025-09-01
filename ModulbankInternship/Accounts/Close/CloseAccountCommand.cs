using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Accounts.Close;

public record CloseAccountCommand(Guid AccountId, ExecutorData Executor): ICommand<bool>;