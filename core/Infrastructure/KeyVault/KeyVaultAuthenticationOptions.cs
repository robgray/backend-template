using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.KeyVault;

public class KeyVaultOptions
{
    public static string Key => "KeyVault";
    
    [Required]   
    public required string StorageEncryptionKeyName { get; set; }
    
    [Required]  
    public required string KeyVaultName { get; set; }
}