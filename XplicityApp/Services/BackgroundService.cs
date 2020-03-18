using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.BackgroundFunctions.Interfaces;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
{
    public class BackgroundService : IBackgroundService
    {
        private readonly ITimeService _timeService;
        private readonly IBackgroundEmailSender _backgroundEmailSender;
        private readonly IEmployeeHolidaysBackgroundUpdater _employeeHolidaysBackgroundUpdater;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<BackgroundService> _logger;
        private readonly IBackgroundInventoryUpdater _backgroundInventoryUpdater;

        public BackgroundService(
            ITimeService timeService, 
            IEmployeeHolidaysBackgroundUpdater employeeHolidaysBackgroundUpdater, 
            IEmployeeRepository employeeRepository, 
            IBackgroundEmailSender backgroundEmailSender, 
            ILogger<BackgroundService> logger,
            IBackgroundInventoryUpdater backgroundInventoryUpdater)
        {
            _timeService = timeService;
            _employeeHolidaysBackgroundUpdater = employeeHolidaysBackgroundUpdater;
            _employeeRepository = employeeRepository;
            _backgroundEmailSender = backgroundEmailSender;
            _logger = logger;
            _backgroundInventoryUpdater = backgroundInventoryUpdater;
        }
        public async Task DoBackgroundTasks()
        {
            _logger.LogInformation("Background tasks were initiated at " + _timeService.GetCurrentTime());

            await _backgroundEmailSender.SendHolidayReports();

            await _backgroundEmailSender.BroadcastCoworkersAbsences();

            await _backgroundEmailSender.BroadcastCoworkersBirthdays();

            var allEmployees = await _employeeRepository.GetAll();

            await _employeeHolidaysBackgroundUpdater.AddFreeWorkDays(allEmployees);

            await _employeeHolidaysBackgroundUpdater.ResetParentalLeaves(allEmployees);

            await _backgroundInventoryUpdater.ApplyDepreciationToInventoryItems();

            _logger.LogInformation("Background tasks ended at " + _timeService.GetCurrentTime());
        }
    }
}
