using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogsService _auditLogsService;

        public AuditLogsController(IAuditLogsService auditLogsService)
        {
            _auditLogsService = auditLogsService;
        }

        public async Task<IActionResult> GetAll()
        {
            var items = await _auditLogsService.GetAll();

            return Ok(items);
        }

        [HttpGet("Page")]
        public async Task<IActionResult> GetPage(int page, int pageSize)
        {
            var items = await _auditLogsService.GetPage(page, pageSize);

            return Ok(items);
        }

        [HttpGet("ByEntity")]
        public async Task<IActionResult> GetByEntityType(string entityType, int page, int pageSize)
        {
            var items = await _auditLogsService.GetByEntityType(entityType, page, pageSize);

            return Ok(items);
        }
    }
}
