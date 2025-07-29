using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Wallets.Interfaces;

namespace ModulbankInternship.Wallets;

public class WalletsRepository: BaseRepository<WalletModel>, IWalletsRepository
{
    public WalletModel[] GetAllByOwner(Guid ownerId)
    {
        return dataBase.Where(w => w.OwnerId == ownerId).ToArray();
    }
}