using ModulbankInternship.Account.Exceptions;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions.Models;
using ModulbankInternship.Accounts.Interfaces;

namespace ModulbankInternship.Transactions;

public class TransactionsRepository : BaseRepository<TransactionModel>, ITransactionsRepository
{
    public TransactionModel Get(Guid id)
    {
        var model = GetExistModel(id);
        if (model is null)
        {
            throw new ResourceNotFoundException($"Transaction Id: {id} not found");
        }

        return model;
    }

    public Guid Add(TransactionModel model)
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

    public void Update(TransactionModel model)
    {
        throw new NotImplementedException();
    }
}