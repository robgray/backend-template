using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Infrastructure.Auth;

public static class AuthStartup
{
    public static void AddCustomAuth(this IServiceCollection services)
    {

    }

    public static void ConfigureCustomAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}