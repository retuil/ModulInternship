using FluentValidation;

namespace ModulbankInternship.Accounts.Modify;

public class ModifyAccountValidator : AbstractValidator<ModifyAccountRequest>
{
    public ModifyAccountValidator()
    {
        RuleFor(c => c.NewInterestRate).GreaterThanOrEqualTo(0);
    }
}
