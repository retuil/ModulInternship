using FluentValidation;

namespace ModulbankInternship.Accounts.Create.ForAnyUser;

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