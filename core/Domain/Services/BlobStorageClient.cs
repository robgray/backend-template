namespace Core.Domain.Services;

using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Newtonsoft.Json;

public class BlobStorageClient : IBlobStorageClient
{
    private readonly BlobServiceClient blobServiceClient;

    public BlobStorageClient(BlobServiceClient blobServiceClient)
    {
        this.blobServiceClient = blobServiceClient;
    }

    public async Task UploadJson(string blobName, string containerName, string serializedData)
    {
        var blobClient = await GetBlobClient(containerName, blobName);

        await using var stream = new MemoryStream();
        var streamWriter = new StreamWriter(stream);
        await streamWriter.WriteAsync(serializedData);
        await streamWriter.FlushAsync();
        stream.Position = 0;
        await blobClient.UploadAsync(stream, overwrite: true);
    }

    public async Task UploadJson<T>(string blobName, string containerName, T data) where T : class
    {
        var serializedData = JsonConvert.SerializeObject(data);

        await UploadJson(blobName, containerName, serializedData);
    }

    public async Task<T> DownloadJson<T>(string blobName, string containerName) where T : class
    {
        var blobClient = await GetBlobClient(containerName, blobName);

        var downloadInfo = await blobClient.DownloadAsync();

        await using var ms = new MemoryStream();
        await downloadInfo.Value.Content.CopyToAsync(ms);
        var result = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(ms.ToArray()));

        return result;
    }

    public async Task<T> DownloadOrInsertJson<T>(string blobName, string containerName, T sampleData) where T : class
    {
        var blobClient = await GetBlobClient(containerName, blobName);

        if (!await blobClient.ExistsAsync()) await UploadJson(blobName, containerName, sampleData);

        var downloadInfo = await blobClient.DownloadAsync();

        await using var ms = new MemoryStream();
        await downloadInfo.Value.Content.CopyToAsync(ms);
        var result = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(ms.ToArray()));

        return result;
    }

    private async Task<BlobClient> GetBlobClient(string containerName, string blobName)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        return containerClient.GetBlobClient(blobName);
    }
}