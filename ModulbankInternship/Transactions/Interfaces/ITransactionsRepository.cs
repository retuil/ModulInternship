using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions;
using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Wallets.Interfaces;

public interface ITransactionsRepository: IRepository<TransactionModel>
{}