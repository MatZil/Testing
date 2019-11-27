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
        private readonly IHolidaysRepository _holidaysRepo;
        private readonly IEmployeeRepository _employeesRepo;

        public DocxGeneratorService(IDocxGenerator docxGenerator, IHolidaysRepository holidaysRepo, IEmployeeRepository employeesRepo)
        {
            _docxGenerator = docxGenerator;
            _holidaysRepo = holidaysRepo;
            _employeesRepo = employeesRepo;
        }

        public async Task<string> GenerateHolidayDocx(int holidayId, HolidayDocumentType holidayDocumentType)
        {
            var holiday = await _holidaysRepo.GetById(holidayId);
            var employee = await _employeesRepo.GetById(holiday.EmployeeId);
            return await _docxGenerator.GenerateDocx(holiday, employee, holidayDocumentType);
        }
    }
}
