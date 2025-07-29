using ModulbankInternship.Infrastructure;
using ModulbankInternship.Wallets.Interfaces;

namespace ModulbankInternship.Transactions;

public class TransactionsRepository: BaseRepository<TransactionModel>, ITransactionsRepository
{ }