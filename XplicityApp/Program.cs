using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using XplicityApp.Services;

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
                    .Build())
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .ConfigureWebHostDefaults(webBuilder => { 
                                                webBuilder.UseStartup<Startup>(); 
                                            })
                .ConfigureServices(services =>
                 {
                     services.AddHostedService<HostedService>();
                 });
        }
    }
}
