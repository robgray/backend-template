namespace Vald.TeleHab.Library.DatabaseUpdater;

using System.Reflection;
using DbUp;
using DbUp.Engine;
using DbUp.Support;
using Serilog;

public class DbUpgradeEngineFactory : IDbUpgradeEngineFactory
{
    private readonly Assembly _executingAssembly;
    private readonly string _connectionString;
    private readonly TimeSpan _timeout;
    private readonly ILogger _logger;

    private readonly string _preDeploymentPrefix;
    private readonly string _deploymentPrefix;
    private readonly string _postDeploymentPrefix;

    public DbUpgradeEngineFactory(string connectionString, TimeSpan timeout, ILogger logger, string? appNamespace)
    {
        _executingAssembly = Assembly.GetExecutingAssembly();
        _connectionString = connectionString;
        _timeout = timeout;
        _logger = logger;

        _preDeploymentPrefix = $"{appNamespace}.PreDeploymentScripts";
        _deploymentPrefix = $"{appNamespace}.DeploymentScripts";
        _postDeploymentPrefix = $"{appNamespace}.PostDeploymentScripts";
    }

    public UpgradeEngine Get() =>
        DeployChanges.To.SqlDatabase(_connectionString)
            .WithExecutionTimeout(_timeout)
            .WithScriptsEmbeddedInAssembly(_executingAssembly, PreDeploymentFilter, PreDeploymentOptions)
            .WithScriptsEmbeddedInAssembly(_executingAssembly, DeploymentFilter, DeploymentOptions)
            .WithScriptsEmbeddedInAssembly(_executingAssembly, PostDeploymentFilter, PostDeploymentOptions)
            .WithTransactionPerScript()
            .LogScriptOutput()
            .LogTo(new SerilogUpgradeLog(_logger))
            .Build();

    private bool PreDeploymentFilter(string resourceName) =>
        resourceName.StartsWith(_preDeploymentPrefix);

    private static SqlScriptOptions PreDeploymentOptions =>
        new() { RunGroupOrder = 0, ScriptType = ScriptType.RunAlways };

    private bool DeploymentFilter(string resourceName) =>
        resourceName.StartsWith(_deploymentPrefix);

    private static SqlScriptOptions DeploymentOptions =>
        new() { RunGroupOrder = 1, ScriptType = ScriptType.RunOnce };

    private bool PostDeploymentFilter(string resourceName) =>
        resourceName.StartsWith(_postDeploymentPrefix);

    private static SqlScriptOptions PostDeploymentOptions =>
        new() { RunGroupOrder = 2, ScriptType = ScriptType.RunAlways };
}