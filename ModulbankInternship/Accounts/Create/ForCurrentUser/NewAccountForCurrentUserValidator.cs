using FluentValidation;
using ModulbankInternship.Accounts.DTO;

namespace ModulbankInternship.Accounts.Create.ForCurrentUser;

public class NewAccountForCurrentUserValidator: AbstractValidator<NewAccountForCurrentUserRequest>
{
    public NewAccountForCurrentUserValidator()
    {
        RuleFor(c => c.Currency).NotEmpty();
        RuleFor(c => c.InterestRate).GreaterThanOrEqualTo(0);
    }
}