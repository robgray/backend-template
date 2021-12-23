using System;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using core.Domain.Services;
using core.Plumbing.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace core.Plumbing.Storage;

public static class StorageStartup
{
    public static void AddCustomAzureStorage(this IServiceCollection services)
    {
        services.AddOptions<StorageOptions>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection(StorageOptions.Key).Bind(settings);
            })
            .ValidateDataAnnotations();

        services.AddTransient(provider =>
        {
            var logger = Log.ForContext(typeof(StorageStartup));

            var keyVaultOptions = provider.GetService<IOptions<KeyVaultOptions>>().Value;
            var storageOptions = provider.GetService<IOptions<StorageOptions>>().Value;
            if (storageOptions.UseEmulator) return new BlobServiceClient("UseDevelopmentStorage=true");

            logger.Information("Using managed identity client {ManagedIdentityClientId} connect to key vault",
                storageOptions.ManagedIdentityClientId);
            var cred = new DefaultAzureCredential(new DefaultAzureCredentialOptions
            {
                ManagedIdentityClientId = storageOptions.ManagedIdentityClientId
            });

            // Only required for client side encryption
            var options = GetBlobClientOptions(keyVaultOptions, cred);

            return new BlobServiceClient(
                new Uri($"https://{storageOptions.StorageAccount}.blob.core.windows.net/"), cred, options);
        });
        services.AddSingleton<IBlobStorageClient, BlobStorageClient>();
    }

    private static BlobClientOptions GetBlobClientOptions(KeyVaultOptions keyVaultOptions, TokenCredential cred)
    {
        var kvUri = "https://" + keyVaultOptions.KeyVaultName + ".vault.azure.net/keys/" +
                    keyVaultOptions.StorageEncryptionKeyName;

        var cryptoClient = new CryptographyClient(new Uri(kvUri), cred);
        var keyResolver = new KeyResolver(cred);
        var encryptionOptions = new ClientSideEncryptionOptions(ClientSideEncryptionVersion.V1_0)
        {
            KeyEncryptionKey = cryptoClient,
            KeyResolver = keyResolver,
            KeyWrapAlgorithm = "RSA-OAEP"
        };
        
        var options = new SpecializedBlobClientOptions
        {
            ClientSideEncryption = encryptionOptions
        };
        
        return options;
    }
}