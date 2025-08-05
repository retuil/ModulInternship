using FluentValidation;
using ModulbankInternship.Account;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ModulbankInternship.Accounts.Validators;

public class ModifyAccountValidator : AbstractValidator<ModifyAccountRequest>
{
    public ModifyAccountValidator()
    {
        RuleFor(c => c.NewInterestRate).GreaterThanOrEqualTo(0);
    }
}
