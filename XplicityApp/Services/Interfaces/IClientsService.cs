using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Dtos.Clients;

namespace XplicityApp.Services.Interfaces
{
    public interface IClientsService
    {
        Task<GetClientDto> GetById(int id);
        Task<ICollection<GetClientDto>> GetAll();
        Task<NewClientDto> Create(NewClientDto newClient);
        Task Update(int id, NewClientDto updateData);
        Task<bool> Delete(int id);
    }
}
