using ModulbankInternship.Account;
using ModulbankInternship.Transactions;
using ModulbankInternship.Transactions.Enums;
using ModulbankInternship.Transactions.Models;
using ModulbankInternship.Accounts.Interfaces;

namespace ModulbankInternship.Accounts;

public class AccountHelper(IAccountsRepository AccountRepository, ITransactionsRepository transactionsRepository)
{
    public Guid CreateTransaction(TransactionModel transaction)
    {
        AccountModel Account = AccountRepository.Get(transaction.AccountId);
        Account.Transactions.Add(transaction);
        switch (transaction.Type)
        {
            case ETransactionType.Credit:
            {
                Account.Balance -= transaction.Amount;
                break;
            }
            case ETransactionType.Debit:
            {
                Account.Balance += transaction.Amount;
                break;
            }
            default:
            {
                throw new NotImplementedException();
            }
        }

        AccountRepository.Update(Account);
        return transactionsRepository.Add(transaction);
    }
}