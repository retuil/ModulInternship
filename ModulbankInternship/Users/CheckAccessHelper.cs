using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Exceptions;
using ModulbankInternship.Users.Interfaces;
using ModulbankInternship.Users.Models;

namespace ModulbankInternship.Users;

public class CheckAccessHelper(IUserRepository userRepository)
{
    public void CheckExecutorAccess(Guid ownerId, Guid executorId, IEnumerable<EAccessClass> accessClasses)
    {
        var executor = userRepository.Get(executorId);
        foreach (var accessClass in accessClasses)
        {
            switch (accessClass)
            {
                case EAccessClass.Manager when executor.Role == EUserRole.Manager:
                case EAccessClass.Cashier when executor.Role == EUserRole.Cashier:
                case EAccessClass.Owner when ownerId == executorId:
                case EAccessClass.Anyone:
                {
                    return;
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