namespace ModulbankInternship.Accounts.Domain.Exceptions;

public class AccountBlockedException(Guid accountId): Exception($"Account with id: {accountId} was blocked");