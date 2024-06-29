using Core.Infrastructure.Logging;

namespace Api;

using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

public class Program
{
    public static async Task<int> Main(string[] args)
    {

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateBootstrapLogger(); // <- This means it is temp and get reconfigured/replaced by the host.

        try
        {
            /* set the default culture */
            var defaultCulture = new CultureInfo("en-AU");
            CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
            CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;
            
            Log.Information("Starting API...");

            var host = CreateHostBuilder(args).Build();
            
            await host.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
            return 1;
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .ConfigureKestrel(opt => opt.AddServerHeader = false)
                    .ConfigureAppConfiguration(builder => builder.AddUserSecrets<Program>())
                    .UseStartup<Startup>();
            })
            .ConfigureLogging(logging => logging.ClearProviders())
            .UseSerilog((context, services, configuration) =>
                configuration
                  .ReadFrom.Configuration(context.Configuration)
                  .ReadFrom.Services(services)
                  .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                  .Enrich.FromLogContext()
                  .Enrich.WithMachineName()
                  .Enrich.WithOperationId()
                  .WriteTo.Console());
}
