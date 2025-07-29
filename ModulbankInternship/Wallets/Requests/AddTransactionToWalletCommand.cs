using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions;

namespace ModulbankInternship.Wallets.Requests;

public record AddTransactionToWalletCommand(TransactionModel TransactionModel): ICommand<bool>;