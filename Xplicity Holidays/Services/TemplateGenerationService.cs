using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Enums;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.TemplateGeneration;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class TemplateGenerationService : ITemplateGenerationService
    {
        private readonly ITemplateGeneration _templateGeneration;
        private readonly IHolidaysRepository _holidaysRepo;

        public TemplateGenerationService(ITemplateGeneration templateGeneration, IHolidaysRepository holidaysRepo)
        {
            _templateGeneration = templateGeneration;
            _holidaysRepo = holidaysRepo;
        }

        public async Task<bool> GenerateHolidayDocx(int holidayId, HolidayDocumentType holidayDocumentType)
        {
            var holiday = await _holidaysRepo.GetById(holidayId);
            await _templateGeneration.GenerateFileByTemplate(holiday, holidayDocumentType);
            return true;
        }
    }
}
