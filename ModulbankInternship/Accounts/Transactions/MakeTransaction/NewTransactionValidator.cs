using FluentValidation;
using ModulbankInternship.Accounts.Transactions.MakeTransaction;

namespace ModulbankInternship.Transactions.Validators;

public class NewTransactionValidator: AbstractValidator<NewTransactionRequest>
{
    public NewTransactionValidator()
    {
        RuleFor(c => c.Amount).GreaterThan(0);
        RuleFor(c => c.Description).NotEmpty();
        RuleFor(c => c.Currency).NotEmpty();
        RuleFor(c => c.AccountId).NotEmpty();
    }
}