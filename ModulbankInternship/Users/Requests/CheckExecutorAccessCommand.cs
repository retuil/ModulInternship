using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.Users.Requests;

public record CheckExecutorAccessCommand(Guid OwnerId, Guid ExecutorId, IEnumerable<EAccessClass> AccessClasses) 
    : ICommand<bool>;