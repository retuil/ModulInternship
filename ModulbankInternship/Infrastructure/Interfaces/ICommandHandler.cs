using MediatR;

namespace ModulbankInternship.Infrastructure.Interfaces;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}