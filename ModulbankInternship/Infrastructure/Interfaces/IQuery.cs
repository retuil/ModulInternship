using MediatR;

namespace ModulbankInternship.Infrastructure;

public interface IQuery<out TResponse>: IRequest<TResponse>
{ }