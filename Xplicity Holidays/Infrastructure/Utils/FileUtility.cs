using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Enums;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.Utils.Interfaces;

namespace Xplicity_Holidays.Infrastructure.Utils
{
    public class FileUtility: IFileUtility
    {
        private readonly IHolidaysRepository _holidaysRepository;
        private readonly IConfiguration _configuration;

        public FileUtility(IHolidaysRepository holidaysRepository, IConfiguration configuration)
        {
            _holidaysRepository = holidaysRepository;
            _configuration = configuration;
        }
        public async Task<string> GetGeneratedDocxPath(int holidayId, HolidayDocumentType holidayDocumentType)
        {
            var holiday = await _holidaysRepository.GetById(holidayId);

            var generationPath = Path.Combine(_configuration["DocxGeneration:GenerationDir"],
                                              _configuration["DocxGeneration:NameFormat"]
                                                .Replace("{holidayId}", holiday.Id.ToString())
                                                .Replace("{documentType}", holidayDocumentType.ToString())
                                                .Replace("{holidayType}", holiday.Type.ToString())
                                             );
            return generationPath;
        }

        public string GetFileName(string fullPath)
        {
            var nameStartIndex = fullPath.LastIndexOf('\\') + 1;
            var fileName = fullPath.Substring(nameStartIndex, fullPath.Length - nameStartIndex);
            return fileName;
        }
    }
}
