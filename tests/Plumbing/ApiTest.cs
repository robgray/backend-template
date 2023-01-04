namespace Tests.Plumbing;

using System;
using Api;
using Flurl.Http;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Serilog;
using Serilog.Core;
using Xunit.Abstractions;

public class ApiTest : IDisposable
{
    private readonly ITestOutputHelper _testOutputHelper;
    private Logger _logger;
    private TestServer _server;

    public ApiTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    protected TestServer Server => _server ??= new TestServer(ConfigureTestServer());

    public void Dispose()
    {
        _server?.Dispose();
        _logger?.Dispose();
    }

   
    private IWebHostBuilder ConfigureTestServer()
    {
        IdentityModelEventSource.ShowPII = true;
        var builder = WebHost
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerPostConfigureOptions>());

                // Try and avoid tests failing with 'The specified configuration does not have a telemetry channel'
                // This appears to emanate from ApplicationInsights trying to initialise TelemetryClient.
                // By settings EnableAzureSdkTelemetryListener to false, we hope to prevent AzureSdkDiagnosticListenerSubscriber
                // from being constructed
                services.AddApplicationInsightsTelemetry();
                services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, options) =>
                {
                    module.EnableAzureSdkTelemetryListener = false;
                });
            })
            .UseStartup<Startup>()
            .ConfigureLogging(ConfigureLogging)
            .UseSerilog()
            .ConfigureTestServices(ConfigureTestServices);

        return builder;
    }

    protected virtual void ConfigureTestServices(IServiceCollection services)
    {
    }

    protected virtual void ConfigureLogging(ILoggingBuilder logging)
    {
        _logger = TestSerilogLogger.CreateTestLogger(_testOutputHelper);

        Log.Logger = _logger;

        logging.ClearProviders();
        logging.AddSerilog(_logger);
    }

    protected FlurlClient CreateClient()
    {
        var client = Server.CreateClient();

        return new FlurlClient(client);
    }

    protected T GetService<T>()
    {
        var scope = Server.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }
}