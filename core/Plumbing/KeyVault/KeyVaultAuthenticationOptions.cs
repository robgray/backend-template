using System.ComponentModel.DataAnnotations;

namespace core.Plumbing.KeyVault;

public class KeyVaultOptions
{
    public static string Key => "KeyVault";
    [Required]   
    public string StorageEncryptionKeyName { get; set; }
    [Required]
    public string KeyVaultName { get; set; }
}