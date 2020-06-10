using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("{fileGuid}/download")]
        public async Task<IActionResult> GetFile(string fileGuid)
        {
            var file = await _fileService.GetByGuid(fileGuid);

            if (file is null)
            {
                return BadRequest();
            }

            return GetFile(file.Name, file.Type);
        }

        private IActionResult GetFile(string fileName, FileTypeEnum fileType)
        {
            var fullPath = Path.Combine(_fileService.GetRelativeBlob(fileType), fileName);

            string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            string containerName = _fileService.GetRelativeBlob(fileType);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            var stream = blockBlob.OpenRead();

            return File(stream, "application/docx", fileName);
        }
    }
}