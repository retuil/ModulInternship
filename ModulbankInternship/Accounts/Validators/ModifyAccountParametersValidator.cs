using FluentValidation;
using ModulbankInternship.Accounts.Requests;

namespace ModulbankInternship.Accounts.Validators;

public class ModifyAccountParametersValidator: AbstractValidator<ModifyAccountParametersCommand>
{
    public ModifyAccountParametersValidator()
    {
        RuleFor(c => c.ExecutorId).NotEmpty();
        RuleFor(c => c.Id).NotEmpty();
    }
}