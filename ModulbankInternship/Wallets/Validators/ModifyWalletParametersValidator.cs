using FluentValidation;
using ModulbankInternship.Wallets.Requests;

namespace ModulbankInternship.Wallets.Validators;

public class ModifyWalletParametersValidator: AbstractValidator<ModifyWalletParametersCommand>
{
    public ModifyWalletParametersValidator()
    {
        RuleFor(c => c.ExecutorId).NotEmpty();
        RuleFor(c => c.Id).NotEmpty();
    }
}