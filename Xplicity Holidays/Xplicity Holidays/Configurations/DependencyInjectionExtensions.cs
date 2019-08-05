﻿using Microsoft.Extensions.DependencyInjection;
using Xplicity_Holidays.Infrastructure.Database.Models;
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
                .AddScoped<IRepository<Employee>, EmployeesRepository>()
                .AddScoped<IRepository<Holiday>, HolidaysRepository>()
                .AddScoped<IRepository<Team>, TeamsRepository>();
        }

        public static IServiceCollection AddApplicationDependencies(this IServiceCollection service)
        {
            return service
                .AddScoped<IClientsService, ClientsService>();
        }
    }
}
