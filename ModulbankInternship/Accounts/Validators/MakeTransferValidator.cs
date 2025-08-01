using FluentValidation;
using ModulbankInternship.Accounts.Requests;

namespace ModulbankInternship.Accounts.Validators;

public class MakeTransferValidator: AbstractValidator<MakeTransferCommand>
{
    public MakeTransferValidator()
    {
        RuleFor(c => c.ExecutorId).NotEmpty();
        RuleFor(c => c.TransferRequest.CounterpartyAccountId).NotEmpty();
        RuleFor(c => c.TransferRequest.AccountId).NotEmpty();
        RuleFor(c => c.TransferRequest.Amount).GreaterThan(0);
    }
}