using ModulbankInternship.Account;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Transactions.DTO;
using ModulbankInternship.Users;
using ModulbankInternship.Wallets;
using ModulbankInternship.Wallets.Interfaces;

namespace ModulbankInternship.Transactions;

public class TransactionRequestHandler(
    IWalletsRepository walletRepository,
    CheckAccessHelper checkAccessHelper,
    WalletHelper walletHelper)
{
    public Guid MakeTransaction(NewTransactionRequest request, Guid executorId)
    {
        var ownerId = walletRepository.Get(request.walletId).OwnerId;
        checkAccessHelper.CheckExecutorAccess(ownerId, executorId, new [] { EAccessClass.Manager, EAccessClass.Cashier });
        var newTransaction = new TransactionModel()
        {
            Amount = request.Amount,
            CounterpartyWalletId = request.counterpartyAccountId,
            Currency = request.Currency,
            DateTime = DateTime.Now,
            Description = request.Description,
            Type = request.TransactionType,
            WalletId = request.walletId
        };
        var newTransactionId = walletHelper.CreateTransaction(newTransaction);
        return newTransactionId;
    }
}