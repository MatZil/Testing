using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Configuration;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Enums;
using Xplicity_Holidays.Infrastructure.Utils.Interfaces;

namespace Xplicity_Holidays.Infrastructure.DocxGeneration
{
    public class DocxGenerator : IDocxGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly ITimeService _timeService;

        public DocxGenerator(IConfiguration configuration, ITimeService timeService)
        {
            _configuration = configuration;
            _timeService = timeService;
        }

        public async Task<string> GenerateDocx(Holiday holiday, Employee employee, HolidayDocumentType holidayDocumentType)
        {
            if (holiday is null)
            {
                throw new ArgumentNullException(nameof(holiday), "Holiday does not exist");
            }

            var replacementMap = GetReplacementMap(holiday, employee);

            var templatePath = GetTemplatePath(holidayDocumentType);

            var generationPath = Path.Combine(_configuration["DocxGeneration:GenerationDir"],
                $"{holiday.Id}-{holidayDocumentType.ToString()}{holiday.Type.ToString()}" +
                $"-{_timeService.GetCurrentTime().ToString("yyyy-MM-dd")}.docx");

            return await Task.FromResult(ProcessTemplate(templatePath, generationPath, replacementMap));
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

        private string ProcessTemplate(string templatePath, string generationPath, Dictionary<string, string> replacementMap)
        {
            using (var template = WordprocessingDocument.CreateFromTemplate(templatePath))
            {
                var body = template.MainDocumentPart.Document.Body;
                foreach (var text in body.Descendants<Text>())
                {
                    foreach (var replacementPair in replacementMap)
                    {
                        text.Text = text.Text.Replace(replacementPair.Key, replacementPair.Value);
                    }
                }
                template.SaveAs(generationPath).Close();
                return generationPath;
            }
        }

        private string GetTemplatePath(HolidayDocumentType holidayDocumentType)
        {
            var templateDir = _configuration["DocxGeneration:TemplatesDir"];
            var templatePath = "";

            switch (holidayDocumentType)
            {
                case HolidayDocumentType.Order:
                    templatePath = Path.Combine(templateDir, _configuration["DocxGeneration:OrderTemplateName"]);
                    break;

                case HolidayDocumentType.Request:
                    templatePath = Path.Combine(templateDir, _configuration["DocxGeneration:RequestTemplateName"]);
                    break;
            }

            return templatePath;
        }
    }
}
