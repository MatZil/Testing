using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Infrastructure.Repositories
{
    public interface IFileRepository : IRepository<FileRecord>
    {
        Task<FileRecord> FindByType(FileTypeEnum fileType);
    }
}
