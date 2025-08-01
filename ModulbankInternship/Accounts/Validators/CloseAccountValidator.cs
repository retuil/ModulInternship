using FluentValidation;
using ModulbankInternship.Accounts.Requests;

namespace ModulbankInternship.Accounts.Validators;

public class CloseAccountValidator: AbstractValidator<CloseAccountCommand>
{
    public CloseAccountValidator()
    {
        RuleFor(c => c.ExecutorId).NotEmpty();
        RuleFor(c => c.AccountId).NotEmpty();
    }
}