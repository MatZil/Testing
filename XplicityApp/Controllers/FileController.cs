using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IAzureStorageService _azureStorageService;

        public FileController(IFileService fileService, IAzureStorageService azureStorageService)
        {
            _fileService = fileService;
            _azureStorageService = azureStorageService;
        }

        [HttpPost]
        [Route("Upload")]
        [RequestSizeLimit(1048576)]
        public async Task<IActionResult> Upload()
        {
            var file = Request.Form.Files[0];
            var fileType = (FileTypeEnum) Enum.Parse(typeof(FileTypeEnum), Request.Form["fileType"]);
            await _fileService.Upload(file, fileType);
            return Ok();
        }
        
        [HttpGet("policy")]
        public async Task<IActionResult> GetNewestPolicy()
        {
            const string policyFileName = "Holiday Policy.pdf";
            var downloadInfo = await _azureStorageService.GetBlobDownloadInfo("policy", policyFileName);
            return File(downloadInfo.Content, downloadInfo.ContentType, policyFileName);
        }
    }
}