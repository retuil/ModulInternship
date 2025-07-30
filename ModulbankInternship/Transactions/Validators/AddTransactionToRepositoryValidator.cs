using FluentValidation;
using ModulbankInternship.Transactions.Requests;

namespace ModulbankInternship.Transactions.Validators;

public class AddTransactionToRepositoryValidator: AbstractValidator<AddTransactionToRepositoryCommand>
{
    public AddTransactionToRepositoryValidator()
    {
        RuleFor(c => c.TransactionModel.Amount).GreaterThan(0);
        RuleFor(c => c.TransactionModel.DateTime).NotEmpty();
        RuleFor(c => c.TransactionModel.Currency).NotEmpty();
        RuleFor(c => c.TransactionModel.IsExist).Equal(true);
    }
}