namespace Api.Infrastructure.Options;

public class ApplicationInsightsOptions
{
    public const string Key = "ApplicationInsights";
    
    public string InstrumentationKey { get; set; } = string.Empty;
    
    public string ConnectionString { get; set; } = string.Empty;
}