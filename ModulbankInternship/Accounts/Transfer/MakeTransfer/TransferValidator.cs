using FluentValidation;

namespace ModulbankInternship.Accounts.Transfer;

public class TransferValidator: AbstractValidator<TransferRequest>
{
    public TransferValidator()
    {
        RuleFor(c => c.AccountId).NotEmpty();
        RuleFor(c => c.Amount).GreaterThan(0);
        RuleFor(c => c.CounterpartyAccountId).NotEmpty();
    }
}