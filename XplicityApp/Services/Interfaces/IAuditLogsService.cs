using System.Threading.Tasks;
using XplicityApp.Dtos.AuditLogs;

namespace XplicityApp.Services.Interfaces
{
    public interface IAuditLogsService
    {
        Task<GetAuditLogsDto> GetByType(string entityType, int page, int pageSize);
    }
}
