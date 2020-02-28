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
            var notifcationSettings = await _notificationSettingsSerivce.GetByEmployeeId(employeeId);

            if (notifcationSettings == null)
            {
                return NotFound();
            }

            return Ok(notifcationSettings);
        }

        [HttpPut("{employeeId}")]
        [Produces(typeof(bool))]
        public async Task<IActionResult> Put(int employeeId, UpdateNotificationSettingsDto updateNotificationSettingsDto)
        {
            var isUpdated = await _notificationSettingsSerivce.Update(employeeId, updateNotificationSettingsDto);

            return Ok(isUpdated);
        }

    }
}
