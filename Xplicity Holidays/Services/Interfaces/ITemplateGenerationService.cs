using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xplicity_Holidays.Constants;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface ITemplateGenerationService
    {
        Task TemplateGeneration(int id, HolidayType holidayType, HolidayDocumentType holidayDocumentType);
    }
}
