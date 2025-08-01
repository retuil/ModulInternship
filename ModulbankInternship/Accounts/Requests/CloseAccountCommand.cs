using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Accounts.Requests;

public record CloseAccountCommand(Guid AccountId, Guid ExecutorId): ICommand<bool>;