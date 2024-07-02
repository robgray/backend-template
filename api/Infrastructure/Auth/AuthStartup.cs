using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Infrastructure.Auth;

public static class AuthStartup
{
    public static IServiceCollection AddCustomAuth(this IServiceCollection services)
    {
        return services;
    }

    public static IApplicationBuilder ConfigureCustomAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}