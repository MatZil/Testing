using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.BackgroundFunctions.Interfaces;
using XplicityApp.Services.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System;

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
        private readonly IBackgroundTasksRepository _repository;

        public BackgroundService(
            ITimeService timeService, 
            IEmployeeHolidaysBackgroundUpdater employeeHolidaysBackgroundUpdater, 
            IEmployeeRepository employeeRepository, 
            IBackgroundEmailSender backgroundEmailSender, 
            ILogger<BackgroundService> logger,
            IBackgroundInventoryUpdater backgroundInventoryUpdater,
            IBackgroundTasksRepository repository)
        {
            _timeService = timeService;
            _employeeHolidaysBackgroundUpdater = employeeHolidaysBackgroundUpdater;
            _employeeRepository = employeeRepository;
            _backgroundEmailSender = backgroundEmailSender;
            _logger = logger;
            _backgroundInventoryUpdater = backgroundInventoryUpdater;
            _repository = repository;
        }

        public async Task ExecuteDailyBackgroundTasks()
        {
            var lastLog = await _repository.GetById(1);
            var today = DateTime.Today;

            if (!today.Equals(lastLog.ExecutionDate))
            {
                var omittedDates = GetOmittedDates(lastLog.ExecutionDate, today);

                foreach (var day in omittedDates)
                {
                    await ExecuteTasks(false);
                    lastLog.ExecutionDate = day;
                    await _repository.Update(lastLog);
                }

                await ExecuteTasks(true);
                lastLog.ExecutionDate = today;
                await _repository.Update(lastLog);
            }
        }

        private async Task ExecuteTasks(bool currentDay)
        {
            _logger.LogInformation("Background tasks were initiated at " + _timeService.GetCurrentTime());

            if (currentDay)
            {
                await _backgroundEmailSender.SendHolidayReports();
                await _backgroundEmailSender.BroadcastCoworkersAbsences();
                await _backgroundEmailSender.BroadcastCoworkersBirthdays();
            }

            var allEmployees = await _employeeRepository.GetAll();

            await _employeeHolidaysBackgroundUpdater.AddFreeWorkDays(allEmployees);
            await _employeeHolidaysBackgroundUpdater.ResetParentalLeaves(allEmployees);

            await _backgroundInventoryUpdater.ApplyDepreciationToInventoryItems();

            _logger.LogInformation("Background tasks ended at " + _timeService.GetCurrentTime());
        }

        private List<DateTime> GetOmittedDates(DateTime lastExecutionDate, DateTime today)
        {
            var omittedDates = Enumerable.Range(1, today.Subtract(lastExecutionDate).Days - 1)
                                             .Select(offset => lastExecutionDate.AddDays(offset))
                                             .ToList();

            return omittedDates;
        }
    }
}
