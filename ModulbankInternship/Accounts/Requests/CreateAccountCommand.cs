using System.Windows.Input;
using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.DTO;

namespace ModulbankInternship.Accounts.Requests;

public record CreateAccountCommand(NewAccountForAnyUserRequest NewAccountRequest, ExecutorData Executor): ICommand<Guid>;