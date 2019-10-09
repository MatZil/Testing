using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Infrastructure.GeneratePDFByTemplate
{
    public class GenerateByTemplate
    {
        //public Dictionary<string, string> TemplateValueDictionary { get; set; } Nereik is duombazes galimaa
        private readonly IEmployeesService _employeeService;
        private readonly IMapper _mapper;
        private readonly IHolidaysService _holidaysService;
        //Ko
        public GenerateByTemplate(IEmployeesService employeeService, IHolidaysService holidaysService,  IMapper mapper)
        {
            _employeeService = employeeService;
            _holidaysService = holidaysService;
            _mapper = mapper;
            //TemplateValueDictionary = templateValueDictionary;
        }
        //How to take value from enum
        public async Task<string> GeneratePDFByTemplate(int employeeId, int holidayType)
        {
            //var holidayDto = await _holidaysService.GetById(holidayId);

            var template = Path.Combine("Templates", (holidayDto.Type == HolidayType.Annual).ToString()); // Ka grazina enum skaiciu ar tipa

            if (!File.Exists(Path.Combine("Templates", (holidayDto.Type == HolidayType.Annual).ToString())))
            {
                throw new ArgumentNullException(null, "Specified template does not exist");
            }


            var source = File.ReadAllText(template);
            foreach (var it in TemplateValueDictionary)
            {
                source = source.Replace(it.Key, it.Value);
            }

            var localPath = Path.Combine("Templates", "GeneratedTemplates", $"{(holidayDto.Type == HolidayType.Annual).ToString()}.docx");
            using (WordprocessingDocument document = WordprocessingDocument.CreateFromTemplate(template))
            {
                var body = document.MainDocumentPart.Document.Body;
                foreach (var text in body.Descendants<Text>())
                {
                    foreach (var it in TemplateValueDictionary)
                    {
                        if (text.Text.Contains(it.Key))
                        {
                            text.Text = text.Text.Replace(it.Key, it.Value);
                        }
                    }
                }

                document.SaveAs(localPath).Close();
            }

            return await Task.FromResult(localPath);
        }
    }
}
