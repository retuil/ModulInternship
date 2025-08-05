using ModulbankInternship.Account;
using ModulbankInternship.Account.Exceptions;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Accounts.Interfaces;

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
        maxId = IndexHelper.NextGuid(maxId);
        model.Id = maxId;
        dataBase.Add(model);
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
        var position = dataBase.FindIndex(v => v.Id == model.Id);
        dataBase[position] = model;
    }


    public AccountModel[] GetAllByOwner(Guid ownerId)
    {
        return dataBase.Where(w => w.OwnerId == ownerId).ToArray();
    }
}