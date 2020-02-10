using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public interface ITagsRepository : IRepository<Tag>
    {
        Task<ICollection<Tag>> FindByTitle(string tagTitle);
        Task<bool> TagExists(string tagTitle);
    }
}
