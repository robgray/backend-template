using core.Plumbing.KeyVault;
using core.Plumbing.Logging;
using core.Plumbing.Mediator;
using core.Plumbing.Storage;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace functions;

[UsedImplicitly]
public class Program
{
    public static void Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder
                    .AddUserSecrets<ExampleTimerFunction>()
                    .AddCommandLine(args);
            })
            .ConfigureServices((ctx, services) =>
            {
                services.AddSingleton(ctx.Configuration);
                
                Log.Logger = new LoggerConfiguration()
                    .BuildLoggerFromConfiguration(ctx.Configuration);
                
                services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
                services.AddCustomMediator();
                services.AddCustomAzureStorage();
                services.AddCustomKeyVault();
            })
            .Build();
            
        host.Run();
    }
}