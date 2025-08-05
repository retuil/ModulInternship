using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.DTO;

namespace ModulbankInternship.Accounts.Requests;

public record MakeTransferCommand(TransferRequest TransferRequest, ExecutorData Executor): ICommand<bool>;