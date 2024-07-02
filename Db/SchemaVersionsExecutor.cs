namespace Vald.TeleHab.Library.DatabaseUpdater;

using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Serilog;

public class SchemaVersionsExecutor
{
    // Note the root namespaces is just a project setting and can't be retrieved
    // via the Assembly class
    private const string RootNamespace = "Vald.TeleHab.Library.DatabaseUpdater";

    private readonly Assembly _executingAssembly;
    private readonly ILogger _logger;
    private readonly string _connectionString;

    public SchemaVersionsExecutor(DatabaseUpdaterOptions options, ILogger logger)
    {
        _executingAssembly = Assembly.GetExecutingAssembly();
        _logger = logger;
        _connectionString = options.ConnectionString;
    }

    public async Task ExecuteAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // using BeginTransaction instead of BeginTransactionAsync for SqlTransaction result...
        await using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

        await ExecuteSqlCommandAsync(transaction, "CreateSchemaVersionsTable");
        await ExecuteSqlCommandAsync(transaction, "InsertPreExistingSchemaVersions");
        await ExecuteSqlCommandAsync(transaction, "DropEFMigrationsHistoryTable");

        await transaction.CommitAsync();

        _logger.Information("Migrating from EF Migrations to DbUp is successful");
    }

    private SqlCommand CreateSqlCommand(SqlTransaction transaction, string commandName) =>
        new(GetCommandText(commandName), transaction.Connection, transaction);

    private Task ExecuteSqlCommandAsync(SqlTransaction transaction, string commandName) =>
        CreateSqlCommand(transaction, commandName).ExecuteNonQueryAsync();

    private string GetCommandText(string commandName)
    {
        var streamName = $"{RootNamespace}.SchemaVersionsScripts.{commandName}.sql";

        using var stream = _executingAssembly.GetManifestResourceStream(streamName);
        if (stream is null)
            throw new InvalidOperationException($"Required embedded resource '{streamName}' not found.");

        using StreamReader reader = new(stream);

        return reader.ReadToEnd();
    }
}