using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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

        public DocxGenerator(IConfiguration configuration, ITimeService timeService, IFileUtility fileUtility, IFileService fileService)
        {
            _configuration = configuration;
            _timeService = timeService;
            _fileUtility = fileUtility;
            _fileService = fileService;
        }

        public async Task<int> GenerateDocx(Holiday holiday, Employee employee, FileTypeEnum holidayDocumentType)
        {
            if (holiday is null)
            {
                throw new ArgumentNullException(nameof(holiday), "Holiday does not exist");
            }

            var replacementMap = GetReplacementMap(holiday, employee);

            var templatePath = GetTemplatePath(holidayDocumentType);

            var generationPath = await _fileUtility.GetGeneratedDocxPath(holiday.Id, holidayDocumentType);

            var fileName = _fileUtility.ExtractNameFromPath(generationPath);

            var fileId = await _fileService.CreateFileRecord(fileName, holidayDocumentType);

            await ProcessTemplate(templatePath, generationPath, replacementMap);

            return fileId;
        }

        private Dictionary<string, string> GetReplacementMap(Holiday holiday, Employee employee)
        {
            var overtimeOrderString = "";
            var overtimeRequestString = "";
            if (holiday.OvertimeDays > 0)
            {
                overtimeOrderString = _configuration["DocxGeneration:OvertimeOrder"].Replace("{OVERTIME_HOURS}", Math.Round(holiday.OvertimeHours, 2).ToString());
                overtimeRequestString = _configuration["DocxGeneration:OvertimeRequest"].Replace("{OVERTIME_HOURS}", Math.Round(holiday.OvertimeHours, 2).ToString());
            }

            return new Dictionary<string, string>
            {
                {"{POSITION}", employee.Position},
                {"{FULL_NAME}", $"{employee.Name} {employee.Surname}"},
                {"{CREATION_DATE}", holiday.RequestCreatedDate.ToString("yyyy-MM-dd") },
                {"{CURRENT_DATE}", _timeService.GetCurrentTime().ToString("yyyy-MM-dd")},
                {"{HOLIDAY_ID}", holiday.Id.ToString() },
                {"{HOLIDAY_BEGIN}", holiday.FromInclusive.ToString("yyyy-MM-dd")},
                {"{HOLIDAY_END}", holiday.ToExclusive.AddDays(-1).ToString("yyyy-MM-dd")},
                {"{WORK_DAY_COUNT}", _timeService.GetWorkDays(holiday.FromInclusive, holiday.ToExclusive).ToString()},
                {"{HOLIDAY_TYPE}", TypeToLithuanian(holiday.Type) },
                {"{ORDER_PAID_INFO}", holiday.Paid ? _configuration["DocxGeneration:OrderPaid"] : _configuration["DocxGeneration:OrderUnpaid"] },
                {"{REQUEST_PAID_INFO}", holiday.Paid ? _configuration["DocxGeneration:RequestPaid"] : _configuration["DocxGeneration:RequestUnpaid"] },
                {"{INCREASED_SALARY_REQUEST}", holiday.Paid ? _configuration["DocxGeneration:IncreasedSalaryRequest"] : ""},
                {"{INCREASED_SALARY_ORDER}", holiday.Paid ? _configuration["DocxGeneration:IncreasedSalaryOrder"] : ""},
                {"{OVERTIME_ORDER}", overtimeOrderString },
                {"{OVERTIME_REQUEST}", overtimeRequestString }
            };
        }

        private string TypeToLithuanian(HolidayType typeToTranslate)
        {
            switch (typeToTranslate)
            {
                case HolidayType.Annual:
                    return "kasmetinių";

                case HolidayType.Parental:
                    return "tėvystės-motinystės";

                case HolidayType.Science:
                    return "mokslo";
            }

            return "";
        }

        private async Task ProcessTemplate(string templatePath, string generationPath, Dictionary<string, string> replacementMap)
        {
            var documentTemplateHtml = await GetHtmlTemplateString(templatePath);

            var documentContentHtml = GetContentFromTemplate(documentTemplateHtml, replacementMap);

            await CreateDocxFromHtml(documentContentHtml, generationPath);
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

        private static async Task CreateDocxFromHtml(string htmlString, string generationPath)
        {
            const string altChunkID = "AltChunkId1";
            using var newDocument = WordprocessingDocument.Create(generationPath, WordprocessingDocumentType.Document);
            var mainPart = newDocument.AddMainDocumentPart();
            var document = new Document(new Body());
            document.Save(mainPart);

            var chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Xhtml, altChunkID);
            using (var chunkStream = chunk.GetStream(FileMode.Create, FileAccess.Write))
            {
                using var stringStream = new StreamWriter(chunkStream, Encoding.UTF8);
                await stringStream.WriteAsync(htmlString);
            }

            var altChunk = new AltChunk { Id = altChunkID };
            mainPart.Document.Body.InsertAt(altChunk, 0);
            mainPart.Document.Save();
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
