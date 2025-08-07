using MediatR;

namespace ModulbankInternship.Infrastructure.Interfaces;

public interface ICommand<out TResponse>: IRequest<TResponse>
{
    
}