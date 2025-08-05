using ModulbankInternship.Infrastructure;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Models;
using ModulbankInternship.Infrastructure.Exceptions;

namespace ModulbankInternship.Accounts;

public class AccountsRepository: BaseRepository<AccountModel>, IAccountsRepository
{
    public AccountModel Get(Guid id)
    {
        var model = GetExistModel(id);
        if (model is null)
        {
            throw new ResourceNotFoundException($"Account Id: {id} not found");
        }

        return model;
    }

    public Guid Add(AccountModel model)
    {
        MaxId = IndexHelper.NextGuid(MaxId);
        model.Id = MaxId;
        DataBase.Add(model);
        return model.Id;
    }

    public void Delete(Guid id)
    {
        var model = GetExistModel(id);
        if (model is null)
        {
            return;
        }

        model.IsExist = false;
    }

    public void Update(AccountModel model)
    {
        var position = DataBase.FindIndex(v => v.Id == model.Id);
        DataBase[position] = model;
    }


    public AccountModel[] GetAllByOwner(Guid ownerId)
    {
        return DataBase.Where(w => w.OwnerId == ownerId).ToArray();
    }
}