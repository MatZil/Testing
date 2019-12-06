using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xplicity_Holidays.Infrastructure.Enums;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Controllers
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

        [HttpGet("{fileId}/download")]
        public async Task<IActionResult> GetFile(int fileId)
        {
            var file = await _fileService.GetById(fileId);

            if (file is null)
            {
                return BadRequest();
            }

            return GetFile(file.Name, file.Type);
        }

        private IActionResult GetFile(string fileName, FileTypeEnum fileType)
        {
            var fullPath = Path.Combine(_fileService.GetDirectory(fileType), fileName);

            var stream = new FileStream(fullPath, FileMode.Open);

            return File(stream, "application/docx", fileName);
        }
    }
}