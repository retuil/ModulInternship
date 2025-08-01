using FluentValidation;
using ModulbankInternship.Accounts.Requests;

namespace ModulbankInternship.Accounts.Validators;

public class CreateAccountValidator: AbstractValidator<CreateAccountCommand>
{
    public CreateAccountValidator()
    {
        RuleFor(c => c.ExecutorId).NotEmpty();
        RuleFor(c => c.NewAccountRequest.Currency).NotEmpty();
        RuleFor(c => c.NewAccountRequest.OwnerId).NotEmpty();
        RuleFor(c => c.NewAccountRequest.InterestRate).GreaterThanOrEqualTo(0);
    }
}