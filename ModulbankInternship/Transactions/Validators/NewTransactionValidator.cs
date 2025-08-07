using FluentValidation;
using ModulbankInternship.Transactions.DTO;

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