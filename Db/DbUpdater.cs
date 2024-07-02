namespace Vald.TeleHab.Library.DatabaseUpdater;

using DbUp.Helpers;
using Serilog;

public class DbUpdater : IDbUpdater
{
    private const int OperationSuccess = 0;
    private const int OperationError = -1;

    private readonly IDbUpgradeEngineFactory _dbUpgradeEngineFactory;
    private readonly DatabaseUpdaterOptions _options;
    private readonly ILogger _logger;
    private readonly SchemaVersionsExecutor _schemaVersionsExecutor;

    public DbUpdater(
        IDbUpgradeEngineFactory dbUpgradeEngineFactory,
        DatabaseUpdaterOptions options,
        ILogger logger,
        SchemaVersionsExecutor schemaVersionsExecutor)
    {
        _dbUpgradeEngineFactory = dbUpgradeEngineFactory;
        _options = options;
        _logger = logger;
        _schemaVersionsExecutor = schemaVersionsExecutor;
    }

    public async Task<int> PerformUpdate()
    {
        if (_options.FirstRunOnExistingDb)
            try
            {
                await _schemaVersionsExecutor.ExecuteAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error performing migration from EF Migrations to DbUp");
                return OperationError;
            }

        var upgradeEngine = _dbUpgradeEngineFactory.Get();

        if (_options.DryRun)
        {
            try
            {
                if (!Directory.Exists(_options.ReportPath)) Directory.CreateDirectory(_options.ReportPath);

                var fullReportPath = Path.Combine(_options.ReportPath, _options.ReportName);

                if (File.Exists(fullReportPath)) File.Delete(fullReportPath);

                _logger.Information("Generating upgrade report at {FullReportPath}", fullReportPath);
                upgradeEngine.GenerateUpgradeHtmlReport(fullReportPath);
                _logger.Information("Report generated successfully");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Dry Run failed");
                return OperationError;
            }
        }
        else
        {
            if (!upgradeEngine.IsUpgradeRequired())
            {
                _logger.Information("Database is already up-to-date. Nothing to do");
                return OperationSuccess;
            }

            var result = upgradeEngine.PerformUpgrade();
            if (result.Successful)
            {
                _logger.Information("Database upgrade is successful");
            }
            else
            {
                _logger.Error(result.Error, "Failed to upgrade database");
                return OperationError;
            }
        }

        return OperationSuccess;
    }
}