using System;
using System.Threading.Tasks;
using XplicityApp.Dtos.AuditLogs;
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

        public async Task<GetAuditLogsDto> GetByType(string entityType, int page, int pageSize)
        {
            if(page < 1)
            {
                throw new ArgumentOutOfRangeException("page parameter is less than 1");
            }
            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("pageSize parameter is less than 1");
            }

            GetAuditLogsDto auditLogs = new GetAuditLogsDto();

            if (entityType == null)
            {
                auditLogs.Logs = await _repository.GetPage(page - 1, pageSize);
            }
            else
            {
                auditLogs.Logs = await _repository.GetPageByType(entityType, page - 1, pageSize);
            }

            auditLogs.TotalCount = await GetAllItemsCount(entityType);

            return auditLogs;
        }

        private async Task<int> GetAllItemsCount(string entityType)
        {
            int itemsCount;

            if (entityType == null)
            {
                itemsCount = await _repository.GetAllItemsCount();
            }
            else
            {
                itemsCount = await _repository.GetItemsCountByType(entityType);
            }

            return itemsCount;
        }
    }
}
