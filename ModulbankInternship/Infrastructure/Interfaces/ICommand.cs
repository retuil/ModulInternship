using MediatR;

namespace ModulbankInternship.Infrastructure;

public interface ICommand<out TResponse>: IRequest<TResponse>
{
    
}