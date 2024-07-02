// DbUp uses System.Data.SqlClient by default, Microsoft.Data.SqlClient needs to be explicitly enabled
// https://github.com/DbUp/DbUp/issues/512
#define SUPPORTS_MICROSOFT_SQL_CLIENT

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Vald.TeleHab.Library.DatabaseUpdater;

const string rootNamespace = "Vald.TeleHab.Library.DatabaseUpdater";

var appConfiguration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .AddCommandLine(args)
    .Build();

using var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        var options = new DatabaseUpdaterOptions(appConfiguration);
        services.AddSingleton(options);
        services.AddTransient<IDbUpgradeEngineFactory>(provider =>
            new DbUpgradeEngineFactory(
                options.ConnectionString,
                options.Timeout,
                provider.GetRequiredService<ILogger>(),
                rootNamespace));
        services.AddTransient<IDbUpdater, DbUpdater>();
        services.AddTransient<SchemaVersionsExecutor>();
    })
    .UseSerilog((_, configuration) =>
    {
        configuration.MinimumLevel.Debug()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code);
    })
    .Build();

var updater = host.Services.GetRequiredService<IDbUpdater>();
return await updater.PerformUpdate();