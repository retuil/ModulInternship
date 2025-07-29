using ModulbankInternship.Account;
using ModulbankInternship.Transactions;
using ModulbankInternship.Transactions.Enums;
using ModulbankInternship.Wallets.Interfaces;

namespace ModulbankInternship.Wallets;

public class WalletHelper(IWalletsRepository walletRepository, ITransactionsRepository transactionsRepository)
{
    public Guid CreateTransaction(TransactionModel transaction)
    {
        WalletModel wallet = walletRepository.Get(transaction.WalletId);
        wallet.Transactions.Add(transaction);
        switch (transaction.Type)
        {
            case ETransactionType.Credit:
            {
                wallet.Balance -= transaction.Amount;
                break;
            }
            case ETransactionType.Debit:
            {
                wallet.Balance += transaction.Amount;
                break;
            }
            default:
            {
                throw new NotImplementedException();
            }
        }

        walletRepository.Update(wallet);
        return transactionsRepository.Add(transaction);
    }
}