using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Dtos.EmailTemplates;

namespace XplicityApp.Services.Interfaces
{
    public interface IEmailTemplatesService
    {
        Task<GetEmailTemplateDto> GetById(int id);
        Task<ICollection<GetEmailTemplateDto>> GetAll();
        Task<GetEmailTemplateDto> Create(NewEmailTemplateDto newTemplate);
        Task Update(int id, NewEmailTemplateDto updateData);
        Task<bool> Delete(int id);
    }
}
