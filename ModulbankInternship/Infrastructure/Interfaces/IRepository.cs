namespace ModulbankInternship.Infrastructure.Interfaces;

public interface IRepository<TModel>
{
    public TModel Get(Guid id);
    public Guid Add(TModel model);
    public void Delete(Guid id);
    public void Update(TModel model);
}