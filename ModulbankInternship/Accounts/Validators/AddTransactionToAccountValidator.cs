using FluentValidation;
using ModulbankInternship.Accounts.Requests;

namespace ModulbankInternship.Accounts.Validators;

public class AddTransactionToAccountValidator: AbstractValidator<AddTransactionToAccountCommand>
{
    public AddTransactionToAccountValidator()
    {
        RuleFor(c => c.TransactionModel.DateTime).NotEmpty();
        RuleFor(c => c.TransactionModel.IsExist).Equal(true);
        RuleFor(c => c.TransactionModel.Currency).NotEmpty();
        RuleFor(c => c.TransactionModel.Amount).GreaterThan(0);
        RuleFor(c => c.TransactionModel.Id).NotEmpty();
        RuleFor(c => c.TransactionModel.AccountId).NotEmpty();
    }
}