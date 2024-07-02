namespace Core.Infrastructure.Mediator;

using MediatR;

public interface IAsyncCommand : IRequest<Result> { }

public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }

public interface IAsyncCommandHandler<in TCommand> : IRequestHandler<TCommand, Result> 
	where TCommand : IAsyncCommand
{ }

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> 
	where TCommand : ICommand<TResponse>, IRequest<Result<TResponse>>
{ }