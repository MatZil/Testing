using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using XplicityApp.Services.Interfaces;
using System.Threading;

namespace XplicityApp.Services
{
    public class HostedService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public HostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var backgroundService = scope.ServiceProvider.GetRequiredService<IBackgroundService>();

            while (!cancellationToken.IsCancellationRequested)
            {
                await backgroundService.DoBackgroundTasks();

                await Task.Delay(TimeSpan.FromDays(1));
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
