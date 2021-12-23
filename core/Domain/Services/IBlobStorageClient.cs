using System.Threading.Tasks;

namespace core.Domain.Services;

public interface IBlobStorageClient
{
    Task UploadJson<T>(string blobName, string containerName, T data) where T: class;
    Task UploadJson(string blobName, string containerName, string serializedData);
    Task<T> DownloadJson<T>(string blobName, string containerName) where T: class;
    Task<T> DownloadOrInsertJson<T>(string blobName, string containerName, T sampleData) where T: class;
}
