using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xplicity_Holidays.Dtos.Inventory;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IInventoryCategoryService
    {
        Task<ICollection<GetInventoryCategoryDto>> GetAll();
    }
}
