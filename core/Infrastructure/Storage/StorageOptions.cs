namespace Core.Infrastructure.Storage;

public class StorageOptions
{
    public static readonly string Key = "AzureStorage";

    public bool UseEmulator { get; set; }
    
    public string? StorageAccount { get; set; }
    
    public string? ManagedIdentityClientId { get; set; }
}