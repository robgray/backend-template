namespace Core.Plumbing.Mediator;

using Domain.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

public static class MediatorStartup
{
    public static void AddCustomMediator(this IServiceCollection services)
    {
        services.AddMediatR(typeof(ICommand));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    }
}