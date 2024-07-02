using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Infrastructure.Cors;

public static class CorsStartup
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration["AllowedHosts"] ?? "*";
        services.AddCors(options => options.AddDefaultPolicy(builder => builder.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
        ));

        return services;
    }

    public static IApplicationBuilder ConfigureCustomCors(this IApplicationBuilder app)
    {
        app.UseCors();

        return app;
    }
}