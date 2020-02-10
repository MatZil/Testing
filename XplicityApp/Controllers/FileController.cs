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
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpPost]
        [Route("Upload")]
        [RequestSizeLimit(1048576)]
        public async Task<IActionResult> Upload()
        {
            var file = Request.Form.Files[0];
            var fileType = (FileTypeEnum)Enum.Parse(typeof(FileTypeEnum), Request.Form["fileType"]);
            await _fileService.Upload(file, fileType);
            return Ok();
        }

        [HttpGet]
        [Route("Policy")]
        public async Task<IActionResult> GetNewestPolicy()
        {
            return Ok(await _fileService.GetNewestPolicyPath());
        }
    }
}