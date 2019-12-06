using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Enums;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.Utils.Interfaces;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Infrastructure.Utils
{
    public class FileUtility: IFileUtility
    {
        private readonly IHolidaysRepository _holidaysRepository;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;

        public FileUtility(IHolidaysRepository holidaysRepository, IConfiguration configuration, IFileService fileService)
        {
            _holidaysRepository = holidaysRepository;
            _configuration = configuration;
            _fileService = fileService;
        }

        public async Task<string> GetGeneratedDocxPath(int holidayId, FileTypeEnum holidayDocumentType)
        {
            var holiday = await _holidaysRepository.GetById(holidayId);

            var generationPath = Path.Combine(_fileService.GetDirectory(holidayDocumentType),
                                              _configuration["DocxGeneration:NameFormat"]
                                                .Replace("{holidayId}", holiday.Id.ToString())
                                                .Replace("{documentType}", holidayDocumentType.ToString())
                                                .Replace("{holidayType}", holiday.Type.ToString())
                                             );
            return generationPath;
        }

        public string ExtractNameFromPath(string fullPath)
        {
            var nameStartIndex = fullPath.LastIndexOf('\\') + 1;
            var fileName = fullPath.Substring(nameStartIndex, fullPath.Length - nameStartIndex);
            return fileName;
        }
    }
}
