using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Dtos.Surveys.Questions.Choices;

namespace XplicityApp.Services.Interfaces
{
    public interface IChoicesService
    {
        Task<GetChoiceDto> GetById(int id);
        Task<ICollection<GetChoiceDto>> GetAll();
        Task<NewChoiceDto> Create(NewChoiceDto newChoice);
        Task Update(int id, NewChoiceDto updateData);
        Task<bool> Delete(int id);
    }
}
