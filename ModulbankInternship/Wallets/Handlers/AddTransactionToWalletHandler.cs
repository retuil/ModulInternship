using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions.Enums;
using ModulbankInternship.Wallets.Interfaces;
using ModulbankInternship.Wallets.Requests;

namespace ModulbankInternship.Wallets;

public class AddTransactionToWalletHandler(IWalletsRepository _walletsRepository)
    : ICommandHandler<AddTransactionToWalletCommand, bool>
{
    public Task<bool> Handle(AddTransactionToWalletCommand request, CancellationToken cancellationToken)
    {
        var transaction = request.TransactionModel;
        var wallet = _walletsRepository.Get(transaction.WalletId);
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

        _walletsRepository.Update(wallet);
        return Task.FromResult(true);
    }
}