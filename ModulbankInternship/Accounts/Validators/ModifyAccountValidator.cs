using FluentValidation;
using ModulbankInternship.Accounts.DTO;

namespace ModulbankInternship.Accounts.Validators;

public class ModifyAccountValidator : AbstractValidator<ModifyAccountRequest>
{
    public ModifyAccountValidator()
    {
        RuleFor(c => c.NewInterestRate).GreaterThanOrEqualTo(0);
    }
}
