using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Emailer;
using Xplicity_Holidays.Infrastructure.PdfGeneration;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.Utils;
using Xplicity_Holidays.Infrastructure.Utils.Interfaces;
using Xplicity_Holidays.Services;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Configurations
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddAllDependencies(this IServiceCollection service)
        {
            return service
                .AddInfrastructureDependencies()
                .AddApplicationDependencies();
        }

        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection service)
        {
            return service
                .AddScoped<IRepository<Client>, ClientsRepository>()
                .AddScoped<IFileRepository, FileRepository>()
                .AddScoped<IHolidaysRepository, HolidaysRepository>()
                .AddScoped<IEmailTemplatesRepository, EmailTemplatesRepository>()
                .AddScoped<IEmployeeRepository, EmployeesRepository>()
                .AddScoped<IEmailer, Emailer>()
                .AddScoped<IPdfGenerator, PdfGenerator>()
                .AddSingleton<ITimeService, TimeService>()
                .AddScoped<IInventoryItemRepository, InventoryItemsRepository>()
                .AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()))
                .AddScoped<IRepository<InventoryCategory>, InventoryCategoryRepository>();
        }

        public static IServiceCollection AddApplicationDependencies(this IServiceCollection service)
        {
            return service
                .AddScoped<IClientsService, ClientsService>()
                .AddScoped<IEmployeesService, EmployeesService>()
                .AddScoped<IHolidayInfoService, HolidayInfoService>()
                .AddScoped<IHolidaysService, HolidaysService>()
                .AddScoped<IHolidayConfirmService, HolidayConfirmService>()
                .AddScoped<IPdfService, PdfService>()
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<IEmailService, EmailService>()
                .AddScoped<IBackgroundService, BackgroundService>()
                .AddScoped<IEmailTemplatesService, EmailTemplatesService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IFileService, FileService>()
                .AddScoped<IInventoryItemService, InventoryItemService>()
                .AddScoped<IInventoryCategoryService, InventoryCategoryService>();
        }
    }
}
