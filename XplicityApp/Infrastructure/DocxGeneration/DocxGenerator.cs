using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Infrastructure.DocxGeneration
{
    public class DocxGenerator : IDocxGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly ITimeService _timeService;
        private readonly IFileUtility _fileUtility;
        private readonly IFileService _fileService;
        private readonly IAzureStorageService _azureStorageService;
        private readonly IOvertimeUtility _overtimeUtility;
        private readonly ILogger<DocxGenerator> _logger;

        public DocxGenerator(
            IConfiguration configuration,
            ITimeService timeService,
            IFileUtility fileUtility,
            IFileService fileService,
            IAzureStorageService azureStorageService,
            IOvertimeUtility overtimeUtility,
            ILogger<DocxGenerator> logger)
        {
            _configuration = configuration;
            _timeService = timeService;
            _fileUtility = fileUtility;
            _fileService = fileService;
            _azureStorageService = azureStorageService;
            _overtimeUtility = overtimeUtility;
            _logger = logger;
        }

        public async Task<int> GenerateDocx(Holiday holiday, Employee employee, FileTypeEnum holidayDocumentType)
        {
            if (holiday is null)
            {
                throw new ArgumentNullException(nameof(holiday), "Holiday does not exist");
            }

            try
            {
                var replacementMap = GetReplacementMap(holiday, employee, holidayDocumentType);
                var templatePath = GetTemplatePath(holidayDocumentType);
                var documentFileName = await _fileUtility.GetGeneratedDocxName(holiday.Id, holidayDocumentType);
                var fileName = _fileUtility.ExtractNameFromPath(documentFileName);
                var fileId = await _fileService.CreateFileRecord(fileName, holidayDocumentType);

                await ProcessTemplate(templatePath, documentFileName, replacementMap, holidayDocumentType);

                return fileId;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Document generation failed for {employee.Email}. Exception message: {exception.Message}");

                throw;
            }
        }

        private Dictionary<string, string> GetReplacementMap(Holiday holiday, Employee employee, FileTypeEnum holidayDocumentType)
        {
            var overtimeOrderString = "";
            var overtimeRequestString = "";
            var increasedSalaryString = "";

            if (holiday.Type == HolidayType.Annual)
            {
                if (holiday.OvertimeDays > 0)
                {
                    var overtimeHours = _overtimeUtility.ConvertOvertimeDaysToHours(holiday.OvertimeDays);
                    overtimeOrderString = _configuration["DocxGeneration:OvertimeOrder"]
                        .Replace("{OVERTIME_HOURS}", Math.Round(overtimeHours, 2).ToString());
                    overtimeRequestString = _configuration["DocxGeneration:OvertimeRequest"]
                        .Replace("{OVERTIME_HOURS}", Math.Round(overtimeHours, 2).ToString());
                }

                increasedSalaryString = _configuration["DocxGeneration:IncreasedSalaryRequest"];
            }

            return new Dictionary<string, string>
            {
                {"{HOLIDAY_PURPOSE}", GetTitleByHolidayType(holiday.Type, holidayDocumentType)},
                {"{POSITION}", employee.Position},
                {"{FULL_NAME}", $"{employee.Name} {employee.Surname}"},
                {"{CREATION_DATE}", holiday.RequestCreatedDate.ToString("yyyy-MM-dd") },
                {"{CURRENT_DATE}", _timeService.GetCurrentTime().ToString("yyyy-MM-dd")},
                {"{HOLIDAY_ID}", holiday.Id.ToString() },
                {"{HOLIDAY_BEGIN}", holiday.FromInclusive.ToString("yyyy-MM-dd")},
                {"{HOLIDAY_END}", holiday.ToInclusive.ToString("yyyy-MM-dd")},
                {"{WORK_DAY_COUNT}", _timeService.GetWorkDays(holiday.FromInclusive, holiday.ToInclusive).ToString()},
                {"{HOLIDAY_TYPE}", TypeToLithuanian(holiday.Type,holidayDocumentType) },
                {"{OVERTIME_ORDER}", overtimeOrderString },
                {"{OVERTIME_REQUEST}", overtimeRequestString },
                {"{INCREASED_SALARY_REQUEST}", increasedSalaryString}
            };
        }

        private static string TypeToLithuanian(HolidayType typeToTranslate, FileTypeEnum holidayDocumentType)
        {
            return typeToTranslate switch
            {
                HolidayType.Annual => (holidayDocumentType == FileTypeEnum.Order ? "kasmetines atostogas" : "išleisti mane kasmetinių atostogų"),
                HolidayType.DayForChildren => (holidayDocumentType == FileTypeEnum.Order
                    ? "papildomą poilsio dieną"
                    : "suteikti man papildomą poilsio dieną vaikų priežiūrai"),
                HolidayType.Science => (holidayDocumentType == FileTypeEnum.Order ? "mokslo atostogas" : "išleisti mane mokslo atostogų"),
                HolidayType.Unpaid => (holidayDocumentType == FileTypeEnum.Order ? "neapmokamas atostogas" : "išleisti mane neapmokamų atostogų"),
                _ => "",
            };
        }

        private static string GetTitleByHolidayType(HolidayType holidayType, FileTypeEnum holidayDocumentType)
        {
            switch (holidayDocumentType)
            {
                case FileTypeEnum.Order:
                    if (holidayType == HolidayType.DayForChildren)
                    {
                        return "DĖL PAPILDOMOS POILSIO DIENOS SUTEIKIMO";
                    }
                    else
                    {
                        return "DĖL ATOSTOGŲ SUTEIKIMO";
                    }
                case FileTypeEnum.Request:
                    if (holidayType == HolidayType.DayForChildren)
                    {
                        return "PRAŠYMAS DĖL PAPILDOMOS ATOSTOGŲ DIENOS SUTEIKIMO";
                    }
                    else if (holidayType == HolidayType.Annual)
                    {
                        return "DĖL KASMETINIŲ ATOSTOGŲ";
                    }
                    else if (holidayType == HolidayType.Science)
                    {
                        return "DĖL MOKSLO ATOSTOGŲ";
                    }
                    else if (holidayType == HolidayType.Unpaid)
                    {
                        return "DĖL NEAPMOKAMŲ ATOSTOGŲ";
                    }

                    break;
            }

            return "";
        }

        private async Task ProcessTemplate(string templatePath, string documentFileName,
            Dictionary<string, string> replacementMap, FileTypeEnum holidayDocumentType)
        {
            var documentTemplateHtml = await GetHtmlTemplateString(templatePath);
            var documentContentHtml = GetContentFromTemplate(documentTemplateHtml, replacementMap);
            await using var stream = await CreateDocxFromHtml(documentContentHtml);
            stream.Position = 0;
            await UploadDocxToBlob(documentFileName, holidayDocumentType, stream);
        }

        private static async Task<string> GetHtmlTemplateString(string templatePath)
        {
            using var reader = new StreamReader(templatePath, Encoding.GetEncoding(1252));
            var htmlTemplateString = await reader.ReadToEndAsync();

            return htmlTemplateString;
        }

        private static string GetContentFromTemplate(string templateString, Dictionary<string, string> replacementMap)
        {
            foreach (var replacementPair in replacementMap)
            {
                templateString = templateString.Replace(replacementPair.Key, replacementPair.Value);
            }

            return templateString;
        }

        private static async Task<MemoryStream> CreateDocxFromHtml(string htmlString)
        {
            var memoryStream = new MemoryStream();
            const string altChunkId = "AltChunkId1";
            using var newDocument = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document);
            var mainPart = newDocument.AddMainDocumentPart();
            var document = new Document(new Body());
            document.Save(mainPart);

            var chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Xhtml, altChunkId);
            using (var chunkStream = chunk.GetStream(FileMode.Create, FileAccess.Write))
            {
                using var stringStream = new StreamWriter(chunkStream, Encoding.UTF8);
                await stringStream.WriteAsync(htmlString);
            }

            var altChunk = new AltChunk { Id = altChunkId };
            mainPart.Document.Body.InsertAt(altChunk, 0);
            mainPart.Document.Save();
            return memoryStream;
        }
        
        private async Task UploadDocxToBlob(string documentFileName, FileTypeEnum holidayDocumentType, Stream stream)
        {
            var containerName = _fileService.GetBlobContainerName(holidayDocumentType);
            await _azureStorageService.UploadBlob(containerName, documentFileName,
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document", stream);
        }
        
        private string GetTemplatePath(FileTypeEnum holidayDocumentType)
        {
            var templateDir = _configuration["DocxGeneration:TemplatesDir"];
            var templatePath = "";

            switch (holidayDocumentType)
            {
                case FileTypeEnum.Order:
                    templatePath = Path.Combine(templateDir, _configuration["DocxGeneration:OrderTemplateName"]);
                    break;

                case FileTypeEnum.Request:
                    templatePath = Path.Combine(templateDir, _configuration["DocxGeneration:RequestTemplateName"]);
                    break;
            }

            return templatePath;
        }
    }
}
