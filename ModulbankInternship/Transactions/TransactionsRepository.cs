using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions.Models;
using ModulbankInternship.Accounts.Interfaces;

namespace ModulbankInternship.Transactions;

public class TransactionsRepository: BaseRepository<TransactionModel>, ITransactionsRepository
{ }