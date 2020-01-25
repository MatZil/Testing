using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public interface IRepository<TEntity> where TEntity: BaseEntity
    {
        Task<ICollection<TEntity>> GetAll();
        Task<TEntity> GetById(int id);
        Task<int> Create(TEntity entity);
        Task<bool> Update(TEntity entity);
        Task<bool> Delete(TEntity entity);
    }
}
