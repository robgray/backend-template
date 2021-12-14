using core.Domain.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace core.Plumbing.Mediator
{
    public static class MediatorStartup
    {
        public static void AddCustomMediator(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ICommand));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        }
    }
}