using FluentValidation;
using ModulbankInternship.Accounts.DTO;

namespace ModulbankInternship.Accounts.Validators;

public class NewAccountForAnyUserValidator: AbstractValidator<NewAccountForAnyUserRequest>
{
    public NewAccountForAnyUserValidator()
    {
        RuleFor(c => c.Currency).NotEmpty();
        RuleFor(c => c.InterestRate).GreaterThanOrEqualTo(0);
        RuleFor(c => c.AccountType).NotEmpty();
        RuleFor(c => c.OwnerId).NotEmpty();
    }
}