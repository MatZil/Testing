using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public FileController(IFileService fileService, IAzureStorageService azureStorageService,
            IConfiguration configuration)
        {
            _fileService = fileService;
            _azureStorageService = azureStorageService;
            _configuration = configuration;
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
            var policyFileName = _configuration["FileConfig:HolidayPolicyFileName"];
            var downloadInfo = await _azureStorageService.GetBlobDownloadInfo("policy", policyFileName);
            return File(downloadInfo.Content, downloadInfo.ContentType, policyFileName);
        }
    }
}