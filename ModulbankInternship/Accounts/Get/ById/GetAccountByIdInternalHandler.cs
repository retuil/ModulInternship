using ModulbankInternship.Accounts.Domain.Exceptions;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Accounts.Get.ById;

public class GetAccountByIdInternalHandler(IGetAccountByIdRepository rep)
    : IQueryHandler<GetAccountByIdInternalQuery, AccountModel>
{
    public async Task<AccountModel> Handle(GetAccountByIdInternalQuery request, CancellationToken cancellationToken)
    {
        var account = await rep.GetByIdAsync(request.Id);
        if (account is null || !account.IsExist)
        {
            throw new AccountNotFoundException(request.Id);
        }

        if (account.IsFrozen)
        {
            throw new AccountBlockedException(request.Id);
        }

        return account;
    }
}