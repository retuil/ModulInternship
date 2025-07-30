using FluentValidation;
using ModulbankInternship.Auth.Requests;

namespace ModulbankInternship.Auth.Validators;

public class LoginValidator: AbstractValidator<LoginUserCommand>
{
    public LoginValidator()
    {
        RuleFor(c => c.AuthContract.PhoneNumber).NotEmpty().Length(10).Must(value => int.TryParse(value, out _));
        RuleFor(c => c.AuthContract.PasswordHash).NotEmpty();
    }
    
}