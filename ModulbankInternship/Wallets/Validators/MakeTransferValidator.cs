using FluentValidation;
using ModulbankInternship.Wallets.Requests;

namespace ModulbankInternship.Wallets.Validators;

public class MakeTransferValidator: AbstractValidator<MakeTransferCommand>
{
    public MakeTransferValidator()
    {
        RuleFor(c => c.ExecutorId).NotEmpty();
        RuleFor(c => c.TransferRequest.CounterpartyWalletId).NotEmpty();
        RuleFor(c => c.TransferRequest.WalletId).NotEmpty();
        RuleFor(c => c.TransferRequest.Amount).GreaterThan(0);
    }
}