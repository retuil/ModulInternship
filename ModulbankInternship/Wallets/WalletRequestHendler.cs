using Microsoft.AspNetCore.Identity;
using ModulbankInternship.Users.Models;

namespace ModulbankInternship.Account;

public class WalletRequestHendler
{
    public WalletModel GetWalletById(Guid id, Guid executorId)
    {
        WalletModel wallet = WalletRepository.Get(id);
        
        
    }

    public Guid CreateWallet(Guid id, Guid executorId)
    {
        
    }

    public void CloseWallet(Guid id, Guid executorId)
    {
        
    }

    public WalletStatementResponse GetAccountStatement(Guid id, DateTime startDate, DateTime finishDate,
        Guid executorId)
    {
        
    }

    public void MakeTransfer(TransferRequest request, Guid executorId)
    {
        
    }
}