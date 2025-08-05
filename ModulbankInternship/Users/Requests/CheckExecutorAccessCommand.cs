using Abp.Events.Bus.Exceptions;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.DTO;

namespace ModulbankInternship.Users.Requests;

public record CheckExecutorAccessCommand(Guid OwnerId, ExecutorData Executor, IEnumerable<EAccessClass> AccessClasses) 
    : ICommand<bool>;