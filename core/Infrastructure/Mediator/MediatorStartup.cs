using Core.Domain.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Mediator;

public static class MediatorStartup
{
    public static void AddCustomMediator(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<ICommand>());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    }
}