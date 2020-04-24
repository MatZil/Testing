using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public interface IFileRepository : IRepository<FileRecord>
    {
        Task<FileRecord> GetNewestPolicy();
        Task<FileRecord> GetByGuid(string guid);
    }
}
