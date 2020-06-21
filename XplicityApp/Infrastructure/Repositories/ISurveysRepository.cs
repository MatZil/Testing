using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public interface ISurveysRepository : IRepository<Survey>
    {
        Task<Survey> GetByGuid(string guid);
    }
}
