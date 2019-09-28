using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xplicity_Holidays.Dtos.EmailTemplates;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class EmailTemplatesService: IEmailTemplatesService
    {
        private readonly IEmailTemplatesRepository _repository;
        private readonly IMapper _mapper;

        public EmailTemplatesService(IEmailTemplatesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetEmailTemplateDto> GetById(int id)
        {
            var template = await _repository.GetById(id);
            var getDto = _mapper.Map<GetEmailTemplateDto>(template);

            return getDto;
        }

        public async Task<ICollection<GetEmailTemplateDto>> GetAll()
        {
            var templates = await _repository.GetAll();
            var getDtos = _mapper.Map<GetEmailTemplateDto[]>(templates);

            return getDtos;
        }

        public async Task<GetEmailTemplateDto> Create(NewEmailTemplateDto newTemplate)
        {
            var template = _mapper.Map<EmailTemplate>(newTemplate);
            await _repository.Create(template);
            var getDto = _mapper.Map<GetEmailTemplateDto>(template);
            return getDto;
        }

        public async Task Update(int id, NewEmailTemplateDto updateData)
        {
            if (updateData == null)
                throw new ArgumentNullException(nameof(updateData));

            var itemToUpdate = await _repository.GetById(id);

            if (itemToUpdate == null)
                throw new InvalidOperationException();

            _mapper.Map(updateData, itemToUpdate);
            await _repository.Update(itemToUpdate);
        }

        public async Task<bool> Delete(int id)
        {
            var item = await _repository.GetById(id);

            if (item == null)
                return false;

            var deleted = await _repository.Delete(item);
            return deleted;
        }
    }
}
