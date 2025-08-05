using FluentValidation;
using ModulbankInternship.Account;

namespace ModulbankInternship.Accounts.Validators;

public class TransferValidator: AbstractValidator<TransferRequest>
{
    public TransferValidator()
    {
        RuleFor(c => c.AccountId).NotEmpty();
        RuleFor(c => c.Amount).GreaterThan(0);
        RuleFor(c => c.CounterpartyAccountId).NotEmpty();
    }
}