using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;

namespace XplicityApp.Services.Interfaces
{
    public interface IAzureStorageService
    {
        Task<BlobDownloadInfo> GetBlobDownloadInfo(string containerName, string blobName);
        string GetBlobUrl(string containerName, string blobName);
        Task UploadBlob(string containerName, string blobName, string contentType, Stream stream);
    }
}