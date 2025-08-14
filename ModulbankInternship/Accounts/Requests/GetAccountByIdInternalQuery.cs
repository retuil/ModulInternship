using ModulbankInternship.Accounts.Models;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Accounts.Requests;

public record GetAccountByIdInternalQuery(Guid Id): IQuery<AccountModel>;