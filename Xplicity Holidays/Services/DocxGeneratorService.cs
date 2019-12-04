using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Enums;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.DocxGeneration;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class DocxGeneratorService : IDocxGeneratorService
    {
        private readonly IDocxGenerator _docxGenerator;
        private readonly IHolidaysRepository _holidaysRepository;
        private readonly IEmployeeRepository _employeesRepository;

        public DocxGeneratorService(IDocxGenerator docxGenerator, IHolidaysRepository holidaysRepository, IEmployeeRepository employeesRepository)
        {
            _docxGenerator = docxGenerator;
            _holidaysRepository = holidaysRepository;
            _employeesRepository = employeesRepository;
        }

        public async Task<string> GenerateHolidayDocx(int holidayId, HolidayDocumentType holidayDocumentType)
        {
            var holiday = await _holidaysRepository.GetById(holidayId);
            var employee = await _employeesRepository.GetById(holiday.EmployeeId);
            return await _docxGenerator.GenerateDocx(holiday, employee, holidayDocumentType);
        }
    }
}
