using FluentValidation;
using ModulbankInternship.Wallets.Requests;

namespace ModulbankInternship.Wallets.Validators;

public class CreateWalletValidator: AbstractValidator<CreateWalletCommand>
{
    public CreateWalletValidator()
    {
        RuleFor(c => c.ExecutorId).NotEmpty();
        RuleFor(c => c.NewWalletRequest.Currency).NotEmpty();
        RuleFor(c => c.NewWalletRequest.OwnerId).NotEmpty();
        RuleFor(c => c.NewWalletRequest.InterestRate).GreaterThanOrEqualTo(0);
    }
}