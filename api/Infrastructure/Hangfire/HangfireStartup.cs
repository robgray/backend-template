namespace Api.Infrastructure.Hangfire;

using Options;
using global::Hangfire;
using global::Hangfire.Dashboard.BasicAuthorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Data.SqlClient;

public static class HangfireStartup
{
    public static IServiceCollection AddCustomHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config =>
        {
            config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(() => new SqlConnection(configuration.GetConnectionString("Database")));
        });
        services.AddHangfireServer();
        
        return services;
    }

    public static IApplicationBuilder UseCustomHangfire(
        this IApplicationBuilder applicationBuilder,
        IWebHostEnvironment environment,
        AuthenticationOptions authenticationOptions)
    {
        if (environment.IsDevelopment())
        {
            applicationBuilder.UseHangfireDashboard();
        }
        else
        {
            applicationBuilder.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization =
                [
                    new BasicAuthAuthorizationFilter(
                        new BasicAuthAuthorizationFilterOptions
                        {
                            Users =
                            [
                                new BasicAuthAuthorizationUser
                                {
                                    Login = authenticationOptions.DiagnosticsKey,
                                    PasswordClear = authenticationOptions.DiagnosticsKey,
                                }
                            ],
                        })
                ],
            });
        }

        return applicationBuilder;
    }
}