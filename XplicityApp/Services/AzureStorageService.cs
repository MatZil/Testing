using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using XplicityApp.Configurations;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        public string GetBlobUrl(string containerName, string blobName)
        {
            var connectionString = AzureStorageConfiguration.GetConnectionString();
            var account = CloudStorageAccount.Parse(connectionString);
            var blobClient = account.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            var blob = container.GetBlockBlobReference(blobName);
            var sasToken = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddMinutes(5),
            }, new SharedAccessBlobHeaders
            {
                ContentDisposition = $"attachment; filename=\"{blobName}\"",
            });
            var downloadUrl = $"{blob.Uri.AbsoluteUri}{sasToken}";
            return downloadUrl;
        }

        public async Task<BlobDownloadInfo> GetBlobDownloadInfo(string containerName, string blobName)
        {
            var blobClient = GetBlobClient(containerName, blobName);
            var downloadInfo = await blobClient.DownloadAsync();
            return downloadInfo;
        }

        public Task UploadBlob(string containerName, string blobName, string contentType, Stream stream)
        {
            var blobClient = GetBlobClient(containerName, blobName);
            return blobClient.UploadAsync(stream, true);
        }

        private BlobClient GetBlobClient(string containerName, string blobName)
        {
            var connectionString = AzureStorageConfiguration.GetConnectionString();
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            return blobClient;
        }
    }
}