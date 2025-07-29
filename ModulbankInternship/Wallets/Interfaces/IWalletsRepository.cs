using ModulbankInternship.Account;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Wallets.Interfaces;

public interface IWalletsRepository: IRepository<WalletModel>
{
    public WalletModel[] GetAllByOwner(Guid ownerId);
}