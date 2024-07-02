namespace Core.Infrastructure.Mediator;

using MediatR;
using Microsoft.Extensions.DependencyInjection;

public static class MediatorStartup
{
    public static void AddCustomMediator(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<ICommand<Result>>());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    }
}