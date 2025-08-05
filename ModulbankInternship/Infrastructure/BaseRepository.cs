using ModulbankInternship.Account;

namespace ModulbankInternship.Infrastructure;

public class BaseRepository<TModel>
where TModel: IModel
{
    protected readonly List<TModel> dataBase = [];
    protected Guid maxId;
    
    protected TModel? GetExistModel(Guid id)
    {
        return dataBase.Where(v => v.IsExist).FirstOrDefault(v => v.Id == id);
    }
}