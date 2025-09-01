namespace ModulbankInternship.Accounts.Close;

public interface ICloseAccountRepository
{
    public Task DeleteAsync(Guid id);

    public Task BlockAccountAsync(Guid id);

    public Task UnblockAccountAsync(Guid id);
}