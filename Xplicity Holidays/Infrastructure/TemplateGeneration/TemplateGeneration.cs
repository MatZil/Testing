using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Xplicity_Holidays.Infrastructure.TemplateGeneration
{
    public class TemplateGeneration : ITemplateGeneration
    {
        public async Task<string> GenerateFileByTemplate(Holiday holiday, HolidayDocumentType holidayDocumentType)
        {
            if (holiday != null)
            {
                var replacementMap = GenerateReplacementMap(holiday);

                var fileName = GetTemplateName(holiday, holidayDocumentType);

                var source = File.ReadAllText(fileName);
                foreach (var templateWord in replacementMap)
                {
                    source = source.Replace(templateWord.Key, templateWord.Value);
                }

                var localPath = Path.Combine("Templates", "GeneratedTemplates",
                    $"{holiday.Id}-{holidayDocumentType.ToString()}{holiday.Type.ToString()}" +
                    $"-{DateTime.Today.Date.ToShortDateString()}.docx");

                return await Task.FromResult(ProcessTemplate(fileName, localPath, replacementMap));
            }
            throw new ArgumentNullException(nameof(holiday), "Holiday does not exist");
        }

        private Dictionary<string, string> GenerateReplacementMap(Holiday holiday)
        {
            return new Dictionary<string, string>
            {
                {"TITLE", holiday.Employee.Position},
                {"HCREATEDAT", $"{holiday.RequestCreatedDate.ToShortDateString()}" },
                {"FULLNAME", $"{holiday.Employee.Name} {holiday.Employee.Surname}"},
                {"CURRENTDATE", DateTime.Today.Date.ToShortDateString()},
                {"HSTART", holiday.FromInclusive.ToShortDateString()},
                {"HEND", holiday.ToExclusive.ToShortDateString()},
                {"HWORKDAY", $"{(holiday.ToExclusive - holiday.FromInclusive).TotalDays}"},
            };
        }

        private static string ProcessTemplate(string fileName, string localPath, Dictionary<string, string> replacementMap)
        {
            using (var document = WordprocessingDocument.CreateFromTemplate(fileName))
            {
                var body = document.MainDocumentPart.Document.Body;
                foreach (var templateFile in body.Descendants<Text>())
                {
                    foreach (var documentText in replacementMap)
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
