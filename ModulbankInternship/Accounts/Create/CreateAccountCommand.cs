using ModulbankInternship.Accounts.Create.ForAnyUser;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Accounts.Create;

public record CreateAccountCommand(NewAccountForAnyUserRequest NewAccountRequest, ExecutorData Executor): ICommand<Guid>;