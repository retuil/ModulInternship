using FluentValidation;
using ModulbankInternship.Wallets.Requests;

namespace ModulbankInternship.Wallets.Validators;

public class AddTransactionToWalletValidator: AbstractValidator<AddTransactionToWalletCommand>
{
    public AddTransactionToWalletValidator()
    {
        RuleFor(c => c.TransactionModel.DateTime).NotEmpty();
        RuleFor(c => c.TransactionModel.IsExist).Equal(true);
        RuleFor(c => c.TransactionModel.Currency).NotEmpty();
        RuleFor(c => c.TransactionModel.Amount).GreaterThan(0);
        RuleFor(c => c.TransactionModel.Id).NotEmpty();
        RuleFor(c => c.TransactionModel.WalletId).NotEmpty();
    }
}