namespace Vald.TeleHab.Library.DatabaseUpdater;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Extensions.Configuration;

[SuppressMessage("ReSharper", "InvertIf", Justification = "Suggestions result in duplicated code")]
public class DatabaseUpdaterOptions
{
    public DatabaseUpdaterOptions(IConfiguration configuration)
    {
        ConnectionString = GetConnectionString(configuration);
        FirstRunOnExistingDb = GetPreExistingDatabase(configuration);
        ReportName = GetReportName(configuration);
        ReportPath = GetReportPath(configuration);
        Timeout = GetTimeout(configuration);
    }

    public string ConnectionString { get; }

    public bool DryRun => !string.IsNullOrWhiteSpace(ReportPath);

    public bool FirstRunOnExistingDb { get; }

    public string ReportName { get; }

    public string ReportPath { get; }

    public TimeSpan Timeout { get; }

    private static string GetConnectionString(IConfiguration configuration)
    {
        var value =
            configuration.GetSection("connectionString").Value
            ?? configuration.GetConnectionString("Database")
            ?? string.Empty;

        return value.Trim();
    }

    private static bool GetPreExistingDatabase(IConfiguration configuration)
    {
        var section = configuration.GetSection("firstRunOnExistingDb");
        if (section.Exists())
            if (bool.TryParse(section.Value, out var value))
                return value;

        return false;
    }

    private static string GetReportName(IConfiguration configuration)
    {
        var section = configuration.GetSection("reportName");
        if (section.Exists())
        {
            var value = section.Value?.Trim();
            if (!string.IsNullOrWhiteSpace(value))
            {
                const string htmlExtension = ".html";
                if (!value.EndsWith(htmlExtension, StringComparison.OrdinalIgnoreCase)) value += htmlExtension;

                return value;
            }
        }

        return "UpgradeReport.html";
    }

    private static string GetReportPath(IConfiguration configuration)
    {
        // 1st try and get the ConnectionString from the args (added to IConfiguration), as that is how Octopus will inject it.
        // 2nd try and get the ConnectionString from the UserSecrets file for local development.
        // ReSharper disable once VariableHidesOuterVariable
        var section = configuration.GetSection("previewReportPath");
        if (section.Exists())
        {
            var value = section.Value;
            if (value is not null) return value.Trim();
        }

        return string.Empty;
    }

    private static TimeSpan GetTimeout(IConfiguration configuration)
    {
        var section = configuration.GetSection("timeout");
        if (section.Exists())
        {
            const NumberStyles style = NumberStyles.AllowLeadingWhite | NumberStyles.AllowLeadingWhite;
            if (double.TryParse(section.Value, style, CultureInfo.CurrentCulture, out var value))
                return TimeSpan.FromSeconds(value);
        }

        return TimeSpan.FromSeconds(60);
    }
}