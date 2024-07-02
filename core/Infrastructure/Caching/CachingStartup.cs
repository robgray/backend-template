namespace Core.Infrastructure.Caching;

using Microsoft.Extensions.DependencyInjection;

public static class CachingStartup
{
    public static IServiceCollection AddCustomCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();
        
        return services;
    }
}
