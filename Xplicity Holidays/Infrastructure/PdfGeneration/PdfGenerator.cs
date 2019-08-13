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
        private readonly IConfiguration _configuration;
        private readonly IConverter _converter;
        public PdfGenerator(IConfiguration configuration, IConverter converter)
        {
            _configuration = configuration;
            _converter = converter;
        }

        public void GeneratePdf(string htmlString, int holidayId, string pdfType)
        {
            var globalSettings = SetGlobalSettings(holidayId.ToString(), pdfType);
            var objectSettings = SetObjectSettings(htmlString, pdfType);

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            LoadDll();

            _converter.Convert(pdf);
        }

        internal GlobalSettings SetGlobalSettings(string holidayId, string pdfType)
        {
            return new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = _configuration[(pdfType == "request") ? "PdfConfig:RequestTitle" : "PdfConfig:OrderTitle"],
                Out = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey) + 
                                                ((pdfType == "request") ? 
                                                        @"\Pdfs\Requests\Holiday_Request_" + $"{holidayId}.pdf":
                                                        @"\Pdfs\Orders\Holiday_Order_" + $"{holidayId}.pdf")
            };
        }

        internal ObjectSettings SetObjectSettings(string htmlString, string pdfType)
        {
            return new ObjectSettings
            {
                PagesCount = _configuration.GetValue<bool>("PdfConfig:PagesCount"),
                HtmlContent = htmlString,
                WebSettings = {DefaultEncoding = _configuration["PdfConfig:DefaultEncoding"],
                UserStyleSheet = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey) +
                                 ((pdfType == "request") ? @"\StyleSheets\Request.css" : @"\StyleSheets\Order.css")},
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
            var context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(_configuration.GetValue<string>(WebHostDefaults.ContentRootKey) +
                                                    @"\Infrastructure\PdfGeneration\Pdf Gen Helpers\libwkhtmltox.dll");
        }

        internal class CustomAssemblyLoadContext : AssemblyLoadContext
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
    }
}
