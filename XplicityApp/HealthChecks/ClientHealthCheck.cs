using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using AutoMapper;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;

namespace XplicityApp.HealthChecks
{
    public class ClientHealthCheck : IHealthCheck
    {

        private readonly IRepository<Client> _clientRepository;
        private readonly IMapper _mapper;
        public ClientHealthCheck(IRepository<Client> repository, IMapper mapper)
        {
            _clientRepository = repository;
            _mapper = mapper;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var clients = await _clientRepository.GetAll();
                return HealthCheckResult.Healthy();
                
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
        }
    }
}