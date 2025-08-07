using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Infrastructure;

public class BaseRepository<TModel>
where TModel: IModel
{
    protected readonly List<TModel> DataBase = [];
    protected Guid MaxId;
    
    protected TModel? GetExistModel(Guid id)
    {
        return DataBase.Where(v => v.IsExist).FirstOrDefault(v => v.Id == id);
    }
}