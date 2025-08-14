using ModulbankInternship.Accounts.Exceptions;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Models;
using ModulbankInternship.Accounts.Requests;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Accounts.Handlers;

public class GetAccountByIdInternalHandler(IAccountsRepository accountsRepository)
    : IQueryHandler<GetAccountByIdInternalQuery, AccountModel>
{
    public async Task<AccountModel> Handle(GetAccountByIdInternalQuery request, CancellationToken cancellationToken)
    {
        var account = await accountsRepository.GetByIdAsync(request.Id);
        if (account is null || !account.IsExist)
        {
            throw new AccountNotFoundException(request.Id);
        }

        return account;
    }
}