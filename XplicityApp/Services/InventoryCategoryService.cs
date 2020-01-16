using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using XplicityApp.Dtos.Inventory;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
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
