using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using XplicityApp.Services.Interfaces;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using XplicityApp.Infrastructure.Utils.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace XplicityApp.Services
{
    public class TimedDailyTaskHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TimedDailyTaskHostedService> _logger;
        private readonly ITimeService _timeService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TimedDailyTaskHostedService(IServiceProvider serviceProvider, ILogger<TimedDailyTaskHostedService> logger,
                                           ITimeService timeService, IWebHostEnvironment webHostEnvironment)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _timeService = timeService;
            _webHostEnvironment = webHostEnvironment;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(GetType().Name + " has started at " + _timeService.GetCurrentTime());

            _timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private async void TimerCallback(object state)
        {
            if (_webHostEnvironment.IsProduction())
            {
                using var scope = _serviceProvider.CreateScope();
                var backgroundService = scope.ServiceProvider.GetRequiredService<IBackgroundService>();
                await backgroundService.DoBackgroundTasks();

                _logger.LogInformation(GetType().Name + " has done an additional iteration at " + _timeService.GetCurrentTime());
            }
            else
            {
                _logger.LogInformation("Skipping background tasks because not running in production (" + _timeService.GetCurrentTime() + ").");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(GetType().Name + " has stopped at " + _timeService.GetCurrentTime());

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
