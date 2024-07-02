namespace Tests.Plumbing;

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ReplaceWithMock<TService>(this IServiceCollection services)
        where TService : class => services.ReplaceWith(Substitute.For<TService>());

    public static IServiceCollection ReplaceWith<TService>(this IServiceCollection services, TService replacement)
        where TService : class
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(TService));
        services.Remove(descriptor);

        services.AddSingleton(replacement);

        return services;
    }
}
