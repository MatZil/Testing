using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using XplicityApp.Dtos.NotificationSettings;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationSettingsController : ControllerBase
    {
        private readonly INotificationSettingsService _notificationSettingsSerivce;
        public NotificationSettingsController(INotificationSettingsService notificationSettingsSerivce)
        {
            _notificationSettingsSerivce = notificationSettingsSerivce;
        }

        [HttpGet("{employeeId}")]
        [Produces(typeof(NotificationSettingsDto))]
        public async Task<IActionResult> Get(int employeeId)
        {
            var notificationSettings = await _notificationSettingsSerivce.GetByEmployeeId(employeeId);

            if (notificationSettings == null)
            {
                return NotFound();
            }

            return Ok(notificationSettings);
        }

        [HttpPut("{employeeId}")]
        [Produces(typeof(bool))]
        public async Task<IActionResult> Put(int employeeId, NotificationSettingsDto notificationSettingsDto)
        {
            var isUpdated = await _notificationSettingsSerivce.Update(employeeId, notificationSettingsDto);

            return Ok(isUpdated);
        }
    }
}
