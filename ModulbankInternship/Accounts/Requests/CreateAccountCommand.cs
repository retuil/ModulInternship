using System.Windows.Input;
using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Accounts.Requests;

public record CreateAccountCommand(NewAccountRequest NewAccountRequest, Guid ExecutorId): ICommand<Guid>;