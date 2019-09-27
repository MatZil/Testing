using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xplicity_Holidays.Dtos.EmailTemplates;

namespace Xplicity_Holidays.Services.Interfaces
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
