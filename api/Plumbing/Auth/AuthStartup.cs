using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace api.Plumbing.Auth;

public static class AuthStartup
{
    public static void AddCustomAuth(this IServiceCollection services)
    {

    }

    public static void ConfigureCustomAuth(this IApplicationBuilder app)
    {
        // app.UseAuthentication();
        // app.UseAuthorization();
    }
}