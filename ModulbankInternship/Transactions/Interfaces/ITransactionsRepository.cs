using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions;
using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Accounts.Interfaces;

public interface ITransactionsRepository: IRepository<TransactionModel>
{}