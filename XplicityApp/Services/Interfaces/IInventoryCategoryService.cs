using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Dtos.Inventory;

namespace XplicityApp.Services.Interfaces
{
    public interface IInventoryCategoryService
    {
        Task<ICollection<GetInventoryCategoryDto>> GetAll();
    }
}
