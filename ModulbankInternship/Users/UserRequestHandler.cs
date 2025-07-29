using ModulbankInternship.Account;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Wallets.Interfaces;

namespace ModulbankInternship.Users;

public class UserRequestHandler(IWalletsRepository walletsRepository, CheckAccessHelper checkAccessHelper)
{
    public WalletModel[] GetUserWallets(Guid userId, Guid executorId)
    {
        checkAccessHelper.CheckExecutorAccess(userId, executorId, new [] { EAccessClass.Manager });

        var wallets = walletsRepository.GetAllByOwner(userId).ToArray();
        return wallets;
    }
}