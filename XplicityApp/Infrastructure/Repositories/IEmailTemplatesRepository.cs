using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public interface IEmailTemplatesRepository: IRepository<EmailTemplate>
    {
        Task<EmailTemplate> GetByPurpose(string purpose);
    }
}
