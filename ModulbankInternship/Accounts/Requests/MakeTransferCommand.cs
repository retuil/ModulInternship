using ModulbankInternship.Accounts.DTO;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Accounts.Requests;

public record MakeTransferCommand(TransferRequest TransferRequest, ExecutorData Executor): ICommand<bool>;