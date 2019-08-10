using System;
using System.Reflection;
using System.Runtime.Loader;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Xplicity_Holidays.Infrastructure.PdfGeneration
{
    public class PdfGenerator: IPdfGenerator
    {
        private IConfiguration _configuration;
        private IConverter _converter;
        public PdfGenerator(IConfiguration configuration, IConverter converter)
        {
            _configuration = configuration;
            _converter = converter;
        }
        public void GeneratePdf(string htmlString, int holidayId)
        {
            var globalSettings = SetGlobalSettings(holidayId.ToString());
            var objectSettings = SetObjectSettings(htmlString);

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            LoadDll();
            _converter.Convert(pdf);
        }

        internal class CustomAssemblyLoadContext: AssemblyLoadContext
        {
            public IntPtr LoadUnmanagedLibrary(string absolutePath)
            {
                return LoadUnmanagedDll(absolutePath);
            }

            protected override IntPtr LoadUnmanagedDll(String unmanagedDllName)
            {
                return LoadUnmanagedDllFromPath(unmanagedDllName);
            }

            protected override Assembly Load(AssemblyName assemblyName)
            {
                throw new NotImplementedException();

            }
        }

        internal GlobalSettings SetGlobalSettings(string holidayId)
        {
            return new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = _configuration["PdfConfig:RequestTitle"],
                Out = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey) + 
                      @"\Pdfs\Requests/\Holiday_Request" + $"_{holidayId}.pdf"
            };
        }

        internal ObjectSettings SetObjectSettings(string htmlString)
        {
            return new ObjectSettings
            {
                PagesCount = _configuration.GetValue<bool>("PdfConfig:PagesCount"),
                HtmlContent = htmlString,
                WebSettings = {DefaultEncoding = _configuration["PdfConfig:DefaultEncoding"],
                    UserStyleSheet = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey) + @"\StyleSheets\Request.css"},
                HeaderSettings =
                {
                    FontName = _configuration["PdfConfig:FontName"],
                    FontSize = _configuration.GetValue<int>("PdfConfig:FontSize"),
                    Line = _configuration.GetValue<bool>("PdfConfig:Line")
                },
                FooterSettings =
                {
                    FontName = _configuration["PdfConfig:FontName"],
                    FontSize = _configuration.GetValue<int>("PdfConfig:FontSize"),
                    Line = _configuration.GetValue<bool>("PdfConfig:Line")
                }
            };
        }

        internal void LoadDll()
        {
            CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(_configuration.GetValue<string>(WebHostDefaults.ContentRootKey) + @"\Pdf Gen Helpers\libwkhtmltox.dll");
        }
    }
}
