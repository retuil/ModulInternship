using FluentValidation;
using ModulbankInternship.Account;
using ModulbankInternship.Accounts.DTO;

namespace ModulbankInternship.Accounts.Validators;

public class NewAccountForCurrentUserValidator: AbstractValidator<NewAccountForCurrentUserRequest>
{
    public NewAccountForCurrentUserValidator()
    {
        RuleFor(c => c.Currency).NotEmpty();
        RuleFor(c => c.InterestRate).GreaterThanOrEqualTo(0);
        RuleFor(c => c.AccountType).NotEmpty();
    }
}