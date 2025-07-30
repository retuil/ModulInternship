using FluentValidation;
using ModulbankInternship.Wallets.Requests;

namespace ModulbankInternship.Wallets.Validators;

public class CloseWalletValidator: AbstractValidator<CloseWalletCommand>
{
    public CloseWalletValidator()
    {
        RuleFor(c => c.ExecutorId).NotEmpty();
        RuleFor(c => c.WalletId).NotEmpty();
    }
}