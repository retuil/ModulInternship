namespace ModulbankInternship.Infrastructure.Interfaces;

public interface IRepository<TModel>
{
    public Task<TModel?> GetByIdAsync(Guid id);
    public Task<Guid?> AddAsync(TModel model);
    public Task DeleteAsync(Guid id);
    public Task UpdateAsync(TModel model);
}