using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions.Models;
using ModulbankInternship.Wallets.Interfaces;

namespace ModulbankInternship.Transactions;

public class TransactionsRepository: BaseRepository<TransactionModel>, ITransactionsRepository
{ }