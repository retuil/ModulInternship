using MediatR;

namespace ModulbankInternship.Infrastructure;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{}