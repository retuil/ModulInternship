using FluentValidation;
using ModulbankInternship.Auth.Requests;

namespace ModulbankInternship.Auth.Validators;

public class RegistrationValidator: AbstractValidator<RegisterUserCommand>
{
    public RegistrationValidator()
    {
        RuleFor(c => c.Contract.PhoneNumber).NotEmpty().Length(10).Must(value => int.TryParse(value, out _));
        RuleFor(c => c.Contract.PasswordHash).NotEmpty();
    }
}