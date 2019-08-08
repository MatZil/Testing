using Microsoft.Extensions.DependencyInjection;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Emailer;
using Xplicity_Holidays.Infrastructure.Repositories;
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
<<<<<<< HEAD
                .AddScoped<IRepository<Holiday>, HolidaysRepository>()
                .AddScoped<IEmailer, Emailer>()
                .AddScoped<IEmployeeRepository, EmployeesRepository>()
=======

                .AddScoped<IEmployeeRepository, EmployeesRepository>()
                .AddScoped<IRepository<Holiday>, HolidaysRepository>()
                .AddScoped<IEmailer, Emailer>()
>>>>>>> 4e9550d99882d6835adf46ec5de803df80e83896
                .AddScoped<IAuthService, AuthenticationService>();
        }

        public static IServiceCollection AddApplicationDependencies(this IServiceCollection service)
        {
            return service
                .AddScoped<IClientsService, ClientsService>()
                .AddScoped<IEmployeesService, EmployeesService>()
                .AddScoped<IHolidaysService, HolidaysService>()
                .AddScoped<IHolidayConfirmService, HolidayConfirmService>();
        }
    }
}
