using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions.Enums;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Requests;

namespace ModulbankInternship.Accounts;

public class AddTransactionToAccountHandler(IAccountsRepository _AccountsRepository)
    : ICommandHandler<AddTransactionToAccountCommand, bool>
{
    public Task<bool> Handle(AddTransactionToAccountCommand request, CancellationToken cancellationToken)
    {
        var transaction = request.TransactionModel;
        var Account = _AccountsRepository.Get(transaction.AccountId);
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

        _AccountsRepository.Update(Account);
        return Task.FromResult(true);
    }
}