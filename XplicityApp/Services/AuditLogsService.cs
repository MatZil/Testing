using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
{
    public class AuditLogsService : IAuditLogsService
    {
        private readonly IAuditLogsRepository _repository;

        public AuditLogsService(IAuditLogsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<AuditLog>> GetAll()
        {
            var auditLogs = await _repository.GetAll();

            return auditLogs;
        }

        public async Task<ICollection<AuditLog>> GetByEntityType(string entityType, int page, int pageSize)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException("EntityType is null");
            }
            if(page < 1)
            {
                throw new ArgumentOutOfRangeException("page parameter is less than 1");
            }
            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("pageSize parameter is less than 1");
            }

            var auditLogs = await _repository.GetByEntityType(entityType, page - 1, pageSize);

            return auditLogs;
        }

        public async Task<ICollection<AuditLog>> GetPage(int page, int pageSize)
        {
            if (page < 1)
            {
                throw new ArgumentOutOfRangeException("page parameter is less than 1");
            }
            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("pageSize parameter is less than 1");
            }

            var auditLogs = await _repository.GetPage(page - 1, pageSize);

            return auditLogs;
        }
    }
}
