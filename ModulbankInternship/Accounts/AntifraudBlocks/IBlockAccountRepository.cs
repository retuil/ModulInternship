namespace ModulbankInternship.Accounts.AntifraudBlocks;

public interface IBlockAccountRepository
{
    public Task BlockAccountAsync(Guid id);

    public Task UnblockAccountAsync(Guid id);
}