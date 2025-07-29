using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Auth.Exceptions;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Exceptions;
using ModulbankInternship.Users.Interfaces;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Users.Handlers;

public class CheckExecutorAccessHandler(IUserRepository userRepository)
    : ICommandHandler<CheckExecutorAccessCommand, bool>
{
    public Task<bool> Handle(CheckExecutorAccessCommand request, CancellationToken cancellationToken)
    {
        var executor = userRepository.Get(request.ExecutorId);
        if (executor is null)
        {
            throw new UnauthorizedException();
        }
        foreach (var accessClass in request.AccessClasses)
        {
            switch (accessClass)
            {
                case EAccessClass.Manager when executor.Role == EUserRole.Manager:
                case EAccessClass.Cashier when executor.Role == EUserRole.Cashier:
                case EAccessClass.Owner when request.OwnerId == request.ExecutorId:
                case EAccessClass.Anyone:
                {
                    return Task.FromResult(true);
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }

        throw new ForbiddenException();
    }
}