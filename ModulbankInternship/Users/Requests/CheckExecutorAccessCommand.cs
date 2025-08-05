using Abp.Events.Bus.Exceptions;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.DTO;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Users.Enums;

namespace ModulbankInternship.Users.Requests;

public record CheckExecutorAccessCommand(Guid OwnerId, ExecutorData Executor, IEnumerable<EAccessClass> AccessClasses) 
    : ICommand<bool>;