using ModulbankInternship.Infrastructure.Exceptions;

namespace ModulbankInternship.Accounts.Domain.Exceptions;

public class AccountNotFoundException(Guid accountId): ResourceNotFoundException($"No open Account with id: {accountId}");