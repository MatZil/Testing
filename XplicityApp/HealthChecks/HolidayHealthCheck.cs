using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using AutoMapper;
using XplicityApp.Infrastructure.Repositories;

namespace XplicityApp.HealthChecks
{
    public class HolidayHealthCheck : IHealthCheck
    {

        private readonly IHolidaysRepository _holidaysRepository;
        private readonly IMapper _mapper;
        public HolidayHealthCheck(IHolidaysRepository repository, IMapper mapper)
        {
            _holidaysRepository = repository;
            _mapper = mapper;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var holidays = await _holidaysRepository.GetAll();
                return HealthCheckResult.Healthy();
                
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
        }
    }
}