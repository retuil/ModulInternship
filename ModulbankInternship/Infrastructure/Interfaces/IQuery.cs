using MediatR;

namespace ModulbankInternship.Infrastructure.Interfaces;

public interface IQuery<out TResponse>: IRequest<TResponse>
{ }