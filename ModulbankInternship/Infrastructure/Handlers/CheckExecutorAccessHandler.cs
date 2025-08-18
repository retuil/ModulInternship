using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Exceptions;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Users.Handlers;

public class CheckExecutorAccessHandler
    : ICommandHandler<CheckExecutorAccessCommand, bool>
{
    public Task<bool> Handle(CheckExecutorAccessCommand request, CancellationToken cancellationToken)
    {
        if (request.Executor.Role is null)
        {
            throw new ForbiddenException($"The user Id: {request.Executor.UserId} does not have a role");
        }

        var executorRole = ConvertRoleToEnum(request.Executor.Role);
        
        foreach (var accessClass in request.AccessClasses)
        {
            switch (accessClass)
            {
                case EAccessClass.Manager when executorRole == EUserRole.Manager:
                case EAccessClass.Cashier when executorRole == EUserRole.Cashier:
                case EAccessClass.Owner when request.OwnerId == request.Executor.UserId:
                case EAccessClass.Anyone:
                {
                    return Task.FromResult(true);
                }
                case EAccessClass.Manager:
                case EAccessClass.Cashier:
                case EAccessClass.Owner:
                {
                    throw new ForbiddenException($"The user Id: {request.Executor.UserId} ");
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }

        throw new ForbiddenException($"No access rights for user id: {request.Executor.UserId} when make action with a resource owned by id: {request.OwnerId}. User role: {executorRole}. Required role: {string.Join(@"\", request.AccessClasses.Select(x => x.ToString()))}");
    }

    private static EUserRole ConvertRoleToEnum(string? stringRole)
    {
        return stringRole switch
        {
            "Manager" => EUserRole.Manager,
            "Cashier" => EUserRole.Cashier,
            "Client" => EUserRole.Client,
            _ => throw new NotImplementedException($"Role {stringRole} is not implemented")
        };
    }
}