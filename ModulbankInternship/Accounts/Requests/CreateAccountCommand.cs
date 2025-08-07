using System.Windows.Input;
using ModulbankInternship.Accounts.DTO;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Accounts.Requests;

public record CreateAccountCommand(NewAccountForAnyUserRequest NewAccountRequest, ExecutorData Executor): ICommand<Guid>;