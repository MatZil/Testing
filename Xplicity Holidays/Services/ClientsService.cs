using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Xplicity_Holidays.Dtos.Clients;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    
    public class ClientsService : IClientsService
    {
        private readonly IRepository<Client> _repository;
        private readonly IMapper _mapper;

        public ClientsService(IRepository<Client> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetClientDto> GetById(int id)
        {
            var client = await _repository.GetById(id);
            var clientDto = _mapper.Map<GetClientDto>(client);
            return clientDto;
        }

        public async Task<ICollection<GetClientDto>> GetAll()
        {
            var clients = await _repository.GetAll();
            var clientsDto = _mapper.Map<GetClientDto[]>(clients);
            return clientsDto;
        }

        public async Task<NewClientDto> Create(NewClientDto newClientDto)
        {
            if (newClientDto == null) throw new ArgumentNullException(nameof(newClientDto));

            var newClient = _mapper.Map<Client>(newClientDto);
            await _repository.Create(newClient);

            var clientDto = _mapper.Map<NewClientDto>(newClient);
            return clientDto;
        }

        public async Task<bool> Delete(int id)
        {
            var item = await _repository.GetById(id);
            if (item == null)
                return false;

            var deleted = await _repository.Delete(item);
            return deleted;
        }

        public async Task Update(int id, NewClientDto updateData)
        {
            if(updateData == null)
                throw new ArgumentNullException(nameof(updateData));

            var itemToUpdate = await _repository.GetById(id);

            if (itemToUpdate == null)
                throw new InvalidOperationException();

            _mapper.Map(updateData, itemToUpdate);
            await _repository.Update(itemToUpdate);
        }
    }
}
