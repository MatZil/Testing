using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Infrastructure.Utils
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

            var generationPath = Path.Combine(_fileService.GetRelativeDirectory(holidayDocumentType),
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
