using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions.Enums;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Requests;

namespace ModulbankInternship.Accounts;

public class AddTransactionToAccountHandler(IAccountsRepository accountsRepository)
    : ICommandHandler<AddTransactionToAccountCommand, bool>
{
    public Task<bool> Handle(AddTransactionToAccountCommand request, CancellationToken cancellationToken)
    {
        var transaction = request.TransactionModel;
        var account = accountsRepository.Get(transaction.AccountId);
        account.Transactions.Add(transaction);
        switch (transaction.Type)
        {
            case ETransactionType.Credit:
            {
                account.Balance -= transaction.Amount;
                break;
            }
            case ETransactionType.Debit:
            {
                account.Balance += transaction.Amount;
                break;
            }
            default:
            {
                throw new NotImplementedException();
            }
        }

        accountsRepository.Update(account);
        return Task.FromResult(true);
    }
}