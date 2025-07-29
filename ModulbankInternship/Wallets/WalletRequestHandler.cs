using Microsoft.AspNetCore.Identity;
using ModulbankInternship.Account.Exceptions;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Transactions;
using ModulbankInternship.Transactions.Enums;
using ModulbankInternship.Users;
using ModulbankInternship.Users.Exceptions;
using ModulbankInternship.Users.Models;
using ModulbankInternship.Wallets;
using ModulbankInternship.Wallets.Interfaces;

namespace ModulbankInternship.Account;

public class WalletRequestHandler(
    IWalletsRepository walletsRepository,
    CheckAccessHelper checkAccessHelper,
    WalletHelper walletHelper)
{
    public WalletModel GetWalletById(Guid id, Guid executorId)
    {
        WalletModel wallet = walletsRepository.Get(id);
        checkAccessHelper.CheckExecutorAccess(wallet.OwnerId, executorId,
            new[] { EAccessClass.Owner, EAccessClass.Manager, EAccessClass.Cashier });

        return wallet;
    }

    public Guid CreateWallet(NewWalletRequest request, Guid executorId)
    {
        checkAccessHelper.CheckExecutorAccess(request.OwnerId, executorId,
            new[] { EAccessClass.Owner, EAccessClass.Manager });

        return walletsRepository.Add(new WalletModel()
        {
            Currency = request.Currency,
            InterestRate = request.InterestRate,
            OwnerId = request.OwnerId,
            WalletType = request.WalletType,
            OpeningDate = DateTime.Now
        });
    }

    public void CloseWallet(Guid walletId, Guid executorId)
    {
        WalletModel wallet = walletsRepository.Get(walletId);
        checkAccessHelper.CheckExecutorAccess(wallet.OwnerId, executorId,
            new[] { EAccessClass.Owner, EAccessClass.Manager });
        if (!wallet.IsExist)
        {
            throw new ResourceNotFoundException();
        }

        walletsRepository.Delete(walletId);
    }

    public WalletStatementResponse GetWalletStatement(Guid walletId, DateTime startDate, DateTime finishDate,
        Guid executorId)
    {
        WalletModel wallet = walletsRepository.Get(walletId);
        checkAccessHelper.CheckExecutorAccess(wallet.OwnerId, executorId, new [] { EAccessClass.Owner , EAccessClass.Manager});
        var statementTransactions = wallet.Transactions.Where(t => t.DateTime >= startDate && t.DateTime <= finishDate);
        return new WalletStatementResponse()
        {
            CreationDate = DateTime.Now,
            StartDate = startDate,
            FinishDate = finishDate,
            Transactions = statementTransactions
        };
    }

    public Dictionary<string, string> ModifyWalletParameters(Guid walletId, ModifyWalletRequest request,
        Guid executorId)
    {
        WalletModel wallet = walletsRepository.Get(walletId);
        checkAccessHelper.CheckExecutorAccess(wallet.OwnerId, executorId, new [] { EAccessClass.Manager });
        var changeReport = new Dictionary<string, string>();

        if (request.NewInterestRate is not null)
        {
            wallet.InterestRate = request.NewInterestRate;
            changeReport[nameof(wallet.InterestRate)] = wallet.InterestRate.ToString();
        }

        return changeReport;
    }

    public void MakeTransfer(TransferRequest request, Guid executorId)
    {
        WalletModel wallet = walletsRepository.Get(request.WalletId);
        WalletModel counterpartyWallet = walletsRepository.Get(request.WalletId);
        checkAccessHelper.CheckExecutorAccess(wallet.OwnerId, executorId, new [] { EAccessClass.Owner});
        
        var creditTransaction = CreateTransferTransaction(wallet, request, ETransactionType.Credit);
        var debitTransaction = CreateTransferTransaction(counterpartyWallet, request, ETransactionType.Debit);
        walletHelper.CreateTransaction(creditTransaction);
        walletHelper.CreateTransaction(debitTransaction);
    }

    

    private TransactionModel CreateTransferTransaction(WalletModel wallet, TransferRequest request, ETransactionType type)
    {
        return new TransactionModel()
        {
            WalletId = wallet.Id,
            Amount = request.Amount,
            CounterpartyWalletId = request.CounterpartyWalletId,
            Currency = wallet.Currency,
            DateTime = DateTime.Now,
            Description =
                $"{type.ToString()} transaction by transfer from wallet id: {wallet.Id} to wallet id: {request.CounterpartyWalletId}",
            Type = type
        };
    }
}