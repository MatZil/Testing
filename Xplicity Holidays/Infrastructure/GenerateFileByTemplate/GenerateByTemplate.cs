using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Xplicity_Holidays.Constants;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.GeneratePDF;
using Xplicity_Holidays.Infrastructure.Repositories;

namespace Xplicity_Holidays.Infrastructure.GeneratePDFByTemplate
{
    public class GenerateByTemplate : IGenerateByTemplate
    {
        private readonly IEmployeeRepository _repository;

        public GenerateByTemplate(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> GenerateFileByTemplate(int employeeId, HolidayType holidayType, HolidayDocumentType holidayDocumentType)
        {
            var employeeDto = await _repository.GetById(employeeId);
            var holidays = _repository.GetHolidays(employeeId);
            // TODO: Check if holidays filter is correct
            var holiday = holidays.FirstOrDefault(h => h.Type == holidayType);

            var values = new Dictionary<string, string>
            {
                {"TITLE", employeeDto.Position},
                {"FULLNAME", $"{employeeDto.Name} {employeeDto.Surname}"},
                {"CURRENTDATE", DateTime.Today.Date.ToShortDateString()},
                {"HSTART", holiday.FromInclusive.ToShortDateString()},
                {"HEND", holiday.ToExclusive.ToShortDateString()},
                {"HWORKDAY", $"{(holiday.ToExclusive - holiday.FromInclusive).TotalDays}"},
            };

            var fileName = GetTemplateName(employeeId, holidayType, holidayDocumentType);

            var source = File.ReadAllText(fileName);
            foreach (var it in values)
            {
                source = source.Replace(it.Key, it.Value);
            }

            var localPath = Path.Combine("Templates", "GeneratedTemplates", $"{employeeDto.Id}-{holiday.Type.ToString()}" +
                                                                            $"-{DateTime.Today.Date.ToShortDateString()}.docx");
            using (var document = WordprocessingDocument.CreateFromTemplate(fileName))
            {
                var body = document.MainDocumentPart.Document.Body;
                foreach (var text in body.Descendants<Text>())
                {
                    foreach (var it in values)
                    {
                        if (text.Text.Contains(it.Key))
                        {
                            text.Text = text.Text.Replace(it.Key, it.Value);
                        }
                    }
                }
                document.SaveAs(localPath).Close();
            }
            // TODO: What should return?
            return await Task.FromResult(localPath);
        }

        private string GetTemplateName(int employeeId, HolidayType holidayType, HolidayDocumentType holidayDocumentType)
        {
            var holidays = _repository.GetHolidays(employeeId);
            var holiday = holidays.FirstOrDefault(h => h.Type == holidayType);
            if (holiday != null)
            {
                if (holidayDocumentType == HolidayDocumentType.Order)
                {
                    var templatePath = Path.Combine("Templates", $"{holidayDocumentType.ToString()}{holiday.Type.ToString()}.docx");
                    if (!File.Exists(templatePath))
                    {
                        throw new ArgumentNullException(null, "Specified template does not exist");
                    }

                    return templatePath;
                }
                else
                {
                    var templatePath = Path.Combine("Templates", $"{holidayDocumentType.ToString()}{holiday.Type.ToString()}.docx");

                    if (!File.Exists(templatePath))
                    {
                        throw new ArgumentNullException(null, "Specified template does not exist");
                    }

                    return templatePath;
                }
            }
            // TODO: What should return ?
            throw new ArgumentNullException(null,"Holiday does not exist");
        }
    }
}
