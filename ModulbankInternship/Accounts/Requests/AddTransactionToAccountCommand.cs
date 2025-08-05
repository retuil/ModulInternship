using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Transactions;
using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Accounts.Requests;

public record AddTransactionToAccountCommand(TransactionModel TransactionModel): ICommand<bool>;