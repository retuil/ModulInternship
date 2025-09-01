using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Infrastructure.Interfaces;

namespace ModulbankInternship.Accounts.Get.ById;

public record GetAccountByIdInternalQuery(Guid Id): IQuery<AccountModel>;