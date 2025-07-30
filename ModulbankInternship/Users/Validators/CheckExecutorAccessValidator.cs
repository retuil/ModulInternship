using FluentValidation;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Users.Validators;

public class CheckExecutorAccessValidator: AbstractValidator<CheckExecutorAccessCommand>
{
    public CheckExecutorAccessValidator()
    {
        RuleFor(c => c.ExecutorId).NotEmpty();
        RuleFor(c => c.OwnerId).NotEmpty();
        RuleFor(c => c.AccessClasses).NotEmpty();
    }
}