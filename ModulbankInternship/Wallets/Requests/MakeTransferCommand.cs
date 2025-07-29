using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Wallets.Requests;

public record MakeTransferCommand(TransferRequest TransferRequest, Guid ExecutorId): ICommand<bool>;