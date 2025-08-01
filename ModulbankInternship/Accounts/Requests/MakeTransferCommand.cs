using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Accounts.Requests;

public record MakeTransferCommand(TransferRequest TransferRequest, Guid ExecutorId): ICommand<bool>;