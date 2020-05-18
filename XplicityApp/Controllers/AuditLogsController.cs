using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogsService _auditLogsService;

        public AuditLogsController(IAuditLogsService auditLogsService)
        {
            _auditLogsService = auditLogsService;
        }

        [HttpGet("Page")]
        public async Task<IActionResult> GetPage(string entityType, int page, int pageSize)
        {
            var items = await _auditLogsService.GetByType(entityType, page, pageSize);

            return Ok(items);
        }
    }
}
