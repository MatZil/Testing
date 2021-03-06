﻿using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.DependencyInjection;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Emailer;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.DocxGeneration;
using XplicityApp.Infrastructure.Utils;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services;
using XplicityApp.Services.Interfaces;
using XplicityApp.Services.Extensions;
using XplicityApp.Services.Extensions.Interfaces;
using XplicityApp.Services.BackgroundFunctions.Interfaces;
using XplicityApp.Services.BackgroundFunctions;
using XplicityApp.Services.Validations.Interfaces;
using XplicityApp.Services.Validations;

namespace XplicityApp.Configurations
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
                .AddScoped<IInventoryItemRepository, InventoryItemsRepository>()
                .AddScoped<IRepository<InventoryCategory>, InventoryCategoryRepository>()
                .AddScoped<IEmailer, Emailer>()
                .AddScoped<IDocxGenerator, DocxGenerator>()
                .AddScoped<IFileUtility, FileUtility>()
                .AddScoped<IEmployeeHolidaysBackgroundUpdater, EmployeeHolidaysBackgroundUpdater>()
                .AddScoped<IEmployeeHolidaysConfirmationUpdater, EmployeeHolidaysConfirmationUpdater>()
                .AddScoped<IBackgroundEmailSender, BackgroundEmailSender>()
                .AddScoped<IBackgroundInventoryUpdater, BackgroundInventoryUpdater>()
                .AddScoped<IOvertimeUtility, OvertimeUtility>()
                .AddScoped<ITagsRepository, TagsRepository>()
                .AddScoped<IInventoryItemTagsRepository, InventoryItemTagsRepository>()
                .AddScoped<INotificationSettingsRepository, NotificationSettingsRepository>()
                .AddScoped<ISurveysRepository, SurveysRepository>()
                .AddScoped<IQuestionsRepository, QuestionsRepository>()
                .AddScoped<IChoicesRepository, ChoicesRepository>()
                .AddScoped<IAuditLogsRepository, AuditLogsRepository>()
                .AddScoped<IAnswerRepository, AnswerRepository>()
                .AddScoped<IHolidayGuidsRepository, HolidayGuidsRepository>()
                .AddScoped<IBackgroundTasksRepository, BackgroundTasksRepository>()
                .AddSingleton<ITimeService, TimeService>()
                .AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        }

        public static IServiceCollection AddApplicationDependencies(this IServiceCollection service)
        {
            return service
                .AddScoped<IClientsService, ClientsService>()
                .AddScoped<IEmployeesService, EmployeesService>()
                .AddScoped<IHolidayInfoService, HolidayInfoService>()
                .AddScoped<IHolidaysService, HolidaysService>()
                .AddScoped<IHolidayConfirmService, HolidayConfirmService>()
                .AddScoped<IHolidayValidationService, HolidayValidationService>()
                .AddScoped<IDocxGeneratorService, DocxGeneratorService>()
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<IEmailService, EmailService>()
                .AddScoped<IEmailTemplatesService, EmailTemplatesService>()
                .AddScoped<IUserService, UserService>()
                .AddTransient<IAzureStorageService, AzureStorageService>()
                .AddScoped<IFileService, FileService>()
                .AddScoped<IInventoryItemService, InventoryItemService>()
                .AddScoped<IInventoryCategoryService, InventoryCategoryService>()
                .AddScoped<IBackgroundService, BackgroundService>()
                .AddScoped<ITagsService, TagsService>()
                .AddScoped<INotificationSettingsService, NotificationSettingsService>()
                .AddScoped<ISurveysService, SurveysService>()
                .AddScoped<IAuditLogsService, AuditLogsService>()
                .AddScoped<IAnswersService, AnswersService>()
                .AddHostedService<TimedDailyTaskHostedService>();
        }
    }
}
