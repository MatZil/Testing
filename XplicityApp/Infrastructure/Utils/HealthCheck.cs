using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Utils
{
    public class HealthCheck : IHealthCheck
    {
        private readonly IHolidaysRepository _holidaysRepository;
        private readonly IRepository<Client> _clientRepository;
        public HealthCheck(IHolidaysRepository holidayRepository, IRepository<Client> clientRepository)
        {
            _holidaysRepository = holidayRepository;
             _clientRepository = clientRepository;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
                var holidays = await _holidaysRepository.GetAll();
                var clients = await _clientRepository.GetAll();

                return HealthCheckResult.Healthy();
        }
    }
}