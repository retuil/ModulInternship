using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Accounts.Requests;

public record CloseAccountCommand(Guid AccountId, ExecutorData Executor): ICommand<bool>;