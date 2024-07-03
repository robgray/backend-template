namespace Api.Infrastructure.HealthChecks;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public static class HealthChecksStartup
{
	public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
	{
		services.AddHealthChecks();
			// Add more health checks here.
		
		return services;
	}

	public static IApplicationBuilder UseCustomHealthChecks(this IApplicationBuilder app)
	{
		app.UseEndpoints(
            endpoints =>
            {
                // liveness is just a pingable endpoint, it doesn't run any health checks
                endpoints.MapHealthChecks(
                    "/liveness",
                    new HealthCheckOptions
                    {
                        ResultStatusCodes =
                        {
                            [HealthStatus.Healthy] = StatusCodes.Status204NoContent,
                            [HealthStatus.Degraded] = StatusCodes.Status204NoContent,
                            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                        },
                        AllowCachingResponses = false,
                        Predicate = _ => false,
                        ResponseWriter = (_, _) => Task.FromResult(string.Empty),
                    });

                // readiness runs all health checks but only returns a status code to indicate health
                endpoints.MapHealthChecks(
                    "/readiness",
                    new HealthCheckOptions
                    {
                        ResultStatusCodes =
                        {
                            [HealthStatus.Healthy] = StatusCodes.Status204NoContent,
                            [HealthStatus.Degraded] = StatusCodes.Status204NoContent,
                            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                        },
                        AllowCachingResponses = false,
                        ResponseWriter = (_, _) => Task.FromResult(string.Empty),
                    });
                
                // diagnostics runs all health checks and returns the results, it is protected via our diagnostics key
                endpoints.MapHealthChecks(
                        "/diagnostics",
                        new HealthCheckOptions
                        {
                            AllowCachingResponses = false,
                            ResponseWriter = DiagnosticHealthCheckResponseWriter.WriteJsonObject,
                        })
                    .RequireAuthorization("Diagnostics");
            });
		
		return app;
	}
}