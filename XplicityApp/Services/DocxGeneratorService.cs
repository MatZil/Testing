using System.Threading.Tasks;
using XplicityApp.Infrastructure.DocxGeneration;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
{
    public class DocxGeneratorService : IDocxGeneratorService
    {
        private readonly IDocxGenerator _docxGenerator;
        private readonly IHolidaysRepository _holidaysRepository;
        private readonly IEmployeeRepository _employeesRepository;

        public DocxGeneratorService(IDocxGenerator docxGenerator, IHolidaysRepository holidaysRepository, 
                                    IEmployeeRepository employeesRepository)
        {
            _docxGenerator = docxGenerator;
            _holidaysRepository = holidaysRepository;
            _employeesRepository = employeesRepository;
        }

        public async Task<int> GenerateHolidayDocx(int holidayId, FileTypeEnum holidayDocumentType)
        {
            var holiday = await _holidaysRepository.GetById(holidayId);
            var employee = await _employeesRepository.GetById(holiday.EmployeeId);
            return await _docxGenerator.GenerateDocx(holiday, employee, holidayDocumentType);
        }
    }
}
