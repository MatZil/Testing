using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Xplicity_Holidays.Constants;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;

namespace Xplicity_Holidays.Infrastructure.TemplateGeneration
{
    public class TemplateGeneration : ITemplateGeneration
    {
        private readonly IEmployeeRepository _repository;
        
        public TemplateGeneration(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> GenerateFileByTemplate(int employeeId, HolidayType holidayType, HolidayDocumentType holidayDocumentType)
        {
            var employeeDto = await _repository.GetById(employeeId);
            var holidays = _repository.GetHolidays(employeeId);
            var holiday = holidays?.Where(h => h != null).FirstOrDefault(h => h.Type == holidayType);

            if (holiday != null)
            {
                var templateValues = GenerateTemplateValues(holiday, employeeDto);

                var fileName = GetTemplateName(holiday, holidayDocumentType);

                var source = File.ReadAllText(fileName);
                foreach (var templateWord in templateValues)
                {
                    source = source.Replace(templateWord.Key, templateWord.Value);
                }

                var localPath = Path.Combine("Templates", "GeneratedTemplates",
                    $"{holiday.Id}-{holidayDocumentType.ToString()}{holiday.Type.ToString()}" +
                    $"-{DateTime.Today.Date.ToShortDateString()}.docx");

                return await Task.FromResult(ProcessTemplate(fileName, localPath, templateValues));
            }
            throw new ArgumentNullException(nameof(holiday), "Holiday does not exist");
        }

        private static Dictionary<string, string> GenerateTemplateValues(Holiday holiday, Employee employeeDto)
        {
            return new Dictionary<string, string>
            {
                {"TITLE", employeeDto.Position},
                {"HCREATEDAT", $"{holiday.RequestCreatedDate.ToShortDateString()}" },
                {"FULLNAME", $"{employeeDto.Name} {employeeDto.Surname}"},
                {"CURRENTDATE", DateTime.Today.Date.ToShortDateString()},
                {"HSTART", holiday.FromInclusive.ToShortDateString()},
                {"HEND", holiday.ToExclusive.ToShortDateString()},
                {"HWORKDAY", $"{(holiday.ToExclusive - holiday.FromInclusive).TotalDays}"},
            };
        }

        private static string ProcessTemplate(string fileName, string localPath, Dictionary<string, string> templateValues)
        {
            using (var document = WordprocessingDocument.CreateFromTemplate(fileName))
            {
                var body = document.MainDocumentPart.Document.Body;
                foreach (var templateFile in body.Descendants<Text>())
                {
                    foreach (var documentText in templateValues)
                    {
                        if (templateFile.Text.Contains(documentText.Key))
                        {
                            templateFile.Text = templateFile.Text.Replace(documentText.Key, documentText.Value);
                        }
                    }
                }
                document.SaveAs(localPath).Close();
                return localPath;
            }
        }

        private string GetTemplateName(Holiday holiday, HolidayDocumentType holidayDocumentType)
        {
            if (holiday != null)
            {
                var templatePath = Path.Combine("Templates", $"{holidayDocumentType.ToString()}{holiday.Type.ToString()}.docx");

                if (holidayDocumentType == HolidayDocumentType.Order)
                {
                    if (!File.Exists(templatePath))
                    {
                        throw new ArgumentNullException(nameof(templatePath), "Specified template does not exist");
                    }

                    return templatePath;
                }

                if (!File.Exists(templatePath))
                {
                    throw new ArgumentNullException(nameof(templatePath), "Specified template does not exist");
                }

                return templatePath;
            }

            throw new ArgumentNullException(nameof(holiday),"Holiday does not exist");
        }
    }
}
