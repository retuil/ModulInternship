using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions;
using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Wallets.Requests;

public record AddTransactionToWalletCommand(TransactionModel TransactionModel): ICommand<bool>;