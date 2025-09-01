using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Accounts.Transfer.MakeTransfer;

public record MakeTransferCommand(TransferRequest TransferRequest, ExecutorData Executor): ICommand<bool>;