using FluentValidation;
using ModulbankInternship.Transactions.Requests;

namespace ModulbankInternship.Transactions.Validators;

public class MakeTransactionValidator: AbstractValidator<MakeTransactionCommand>
{
    public MakeTransactionValidator()
    {
        RuleFor(c => c.ExecutorId).NotEmpty();
        RuleFor(c => c.NewTransactionRequest.Amount).GreaterThan(0);
        RuleFor(c => c.NewTransactionRequest.Description).NotEmpty();
        RuleFor(c => c.NewTransactionRequest.Currency).NotEmpty();
        RuleFor(c => c.NewTransactionRequest.AccountId).NotEmpty();
    }
}