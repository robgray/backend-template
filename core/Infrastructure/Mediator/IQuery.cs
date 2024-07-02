namespace Core.Infrastructure.Mediator;

using MediatR;

public interface IQuery<out TResponse> : IRequest<TResponse> 
	where TResponse : class { }

public interface IQueryHandler<in TQuery, TData> : IRequestHandler<TQuery, TData> 
	where TQuery : IQuery<TData> 
	where TData : class { }
