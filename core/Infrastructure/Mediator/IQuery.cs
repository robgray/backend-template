namespace Core.Infrastructure.Mediator;

using MediatR;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> 
	where TResponse : class { }

public interface IQueryHandler<in TQuery, TData> : IRequestHandler<TQuery, Result<TData>> 
	where TQuery : IQuery<TData>, IRequest<Result<TData>>
	where TData : class { }
