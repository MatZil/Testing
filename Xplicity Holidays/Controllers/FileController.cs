using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xplicity_Holidays.Infrastructure.Enums;
using Xplicity_Holidays.Infrastructure.Static_Files;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Controllers
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
            FileTypeEnum fileType = (FileTypeEnum)Enum.Parse(typeof(FileTypeEnum), Request.Form["fileType"]);
            var filePath = await _fileService.Upload(file, fileType);
            if (!string.IsNullOrEmpty(filePath))
            {
                return Ok(new { filePath });
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetByType")]
        public async Task<IActionResult> GetByType(FileTypeEnum fileType)
        {
            var path = await _fileService.GetByType(fileType);
            if (string.IsNullOrWhiteSpace(path))
            {
                return BadRequest();
            }

            return Ok(path);
        }
    }
}