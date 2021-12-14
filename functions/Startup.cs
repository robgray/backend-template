using System;
using core.Plumbing.KeyVault;
using core.Plumbing.Logging;
using core.Plumbing.Mediator;
using core.Plumbing.Storage;
using functions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

[assembly: FunctionsStartup(typeof(Startup))]

namespace functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("local.settings.json", true)
                .Build();

            builder.Services.AddSingleton<IConfiguration>(config);

            Log.Logger = new LoggerConfiguration()
                .BuildLoggerFromConfiguration(config, typeof(Startup));

            builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

            builder.Services.AddCustomMediator();

            builder.Services.AddCustomAzureStorage();

            builder.Services.AddCustomKeyVault();
        }
    }
}