namespace ModulbankInternship.Infrastructure;

public class BaseRepository<TModel>: IRepository<TModel>
where TModel: IModel
{
    protected readonly List<TModel> dataBase = [];
    protected Guid maxId;
    
    public virtual TModel? Get(Guid id)
    {
        return GetExistModel(id);
    }

    public virtual Guid Add(TModel model)
    {
        maxId = IndexHelper.NextGuid(maxId);
        model.Id = maxId;
        dataBase.Add(model);
        return model.Id;
    }

    public virtual void Delete(Guid id)
    {
        var model = GetExistModel(id);
        if (model is null)
        {
            return;
        }

        model.IsExist = false;
    }

    public virtual void Update(TModel model)
    {
        var position = dataBase.FindIndex(v => v.Id == model.Id);
        dataBase[position] = model;
    }

    protected TModel? GetExistModel(Guid id)
    {
        return dataBase.Where(v => v.IsExist).FirstOrDefault(v => v.Id == id);
    }
}