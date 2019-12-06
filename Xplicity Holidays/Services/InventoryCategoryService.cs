using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Xplicity_Holidays.Dtos.Inventory;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class InventoryCategoryService : IInventoryCategoryService
    {
        private readonly IRepository<InventoryCategory> _repository;
        private readonly IMapper _mapper;
        public InventoryCategoryService(IRepository<InventoryCategory> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<ICollection<GetInventoryCategoryDto>> GetAll()
        {
            var categories = await _repository.GetAll();
            var categoriesDto = _mapper.Map<GetInventoryCategoryDto[]>(categories);
            return categoriesDto;
        }
    }
}
