using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;
using System.IO;

namespace XplicityApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder => 
                    builder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("email-templates-settings.json")
                    .AddJsonFile("equipment-categories.json")
                    .Build())
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddAzureWebAppDiagnostics();
                })
                .ConfigureServices(serviceCollection => serviceCollection
                    .Configure<AzureBlobLoggerOptions>(options => 
                        options.BlobName = "log.txt"))
                .ConfigureWebHostDefaults(webBuilder => { 
                    webBuilder.UseStartup<Startup>(); 
                });
        }
    }
}
