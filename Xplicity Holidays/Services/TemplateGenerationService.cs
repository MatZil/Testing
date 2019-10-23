using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Xplicity_Holidays.Constants;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.TemplateGeneration;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class TemplateGenerationService : ITemplateGenerationService
    {
        private readonly ITemplateGeneration _templateGeneration;

        public TemplateGenerationService(ITemplateGeneration templateGeneration)
        {
            _templateGeneration = templateGeneration;
        }

        public async Task TemplateGeneration(int id, HolidayType holidayType, HolidayDocumentType holidayDocumentType)
        {
            await _templateGeneration.GenerateFileByTemplate(id, holidayType, holidayDocumentType);
        }
    }
}
