using System.Collections.Generic;
using System.Threading.Tasks;
using Xplicity_Holidays.Dtos.Clients;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IClientsService
    {
        Task<NewClientDto> GetById(int id);
        Task<ICollection<NewClientDto>> GetAll();
        Task<NewClientDto> Create(NewClientDto newClient);
        Task Update(int id, NewClientDto updateData);
        Task<bool> Delete(int id);
    }
}
