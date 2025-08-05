using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.DTO;

namespace ModulbankInternship.Accounts.Requests;

public record CloseAccountCommand(Guid AccountId, ExecutorData Executor): ICommand<bool>;