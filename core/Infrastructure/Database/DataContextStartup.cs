namespace Core.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DataContextStartup
{
    public static IServiceCollection AddCustomDataContext(
        this IServiceCollection services, 
        ConnectionStringsOptions connectionStringsOptions)
    {
        //services.AddEntityFrameworkSqlServer();
        
        services.AddDbContext<DataContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(connectionStringsOptions.Database, o => o.EnableRetryOnFailure());
            options.UseApplicationServiceProvider(serviceProvider);
        });

        return services;
    }
}
