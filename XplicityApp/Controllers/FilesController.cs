using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IAzureStorageService _azureStorageService;

        public FilesController(IFileService fileService, IAzureStorageService azureStorageService)
        {
            _fileService = fileService;
            _azureStorageService = azureStorageService;
        }

        [HttpGet("{fileGuid}/download")]
        public async Task<IActionResult> GetFile(string fileGuid)
        {
            var file = await _fileService.GetByGuid(fileGuid);

            if (file is null)
            {
                return BadRequest();
            }

            var containerName = _fileService.GetBlobContainerName(file.Type);
            var downloadInfo = await _azureStorageService.GetBlobDownloadInfo(containerName, file.Name);
            return File(downloadInfo.Content, downloadInfo.ContentType, file.Name);
        }
    }
}