using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xplicity_Holidays.Infrastructure.Enums;
using Xplicity_Holidays.Infrastructure.Utils.Interfaces;

namespace Xplicity_Holidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly IFileUtility _fileUtility;

        public DownloadController(IFileUtility fileUtility)
        {
            _fileUtility = fileUtility;
        }

        [HttpGet("{fileType}/{holidayId}")]
        public async Task<IActionResult> GetFile(string fileType, int holidayId)
        {
            if (fileType == HolidayDocumentType.Order.ToString().ToLower())
            {
                return await GetFile(holidayId, HolidayDocumentType.Order);
            }
            else if (fileType == HolidayDocumentType.Request.ToString().ToLower())
            {
                return await GetFile(holidayId, HolidayDocumentType.Request);
            }

            return BadRequest();
        }

        private async Task<IActionResult> GetFile(int holidayId, HolidayDocumentType documentType)
        {
            var fullPath = await _fileUtility.GetGeneratedDocxPath(holidayId, documentType);
            var fileName = _fileUtility.GetFileName(fullPath);

            var stream = new FileStream(fullPath, FileMode.Open);

            return File(stream, "application/docx", fileName);
        }
    }
}