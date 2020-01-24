﻿using AutoMapper;
using XplicityApp.Dtos.Clients;
using XplicityApp.Dtos.EmailTemplates;
using XplicityApp.Dtos.Employees;
using XplicityApp.Dtos.Holidays;
using XplicityApp.Dtos.Inventory;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Configurations
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration() : this("Holidays")
        { }

        protected AutoMapperConfiguration(string name) : base(name)
        {
            CreateMap<NewClientDto, Client>(MemberList.None);
            CreateMap<Client, NewClientDto>(MemberList.None);

            CreateMap<NewEmployeeDto, Employee>(MemberList.None);
            CreateMap<Employee, NewEmployeeDto>(MemberList.None);

            CreateMap<GetEmployeeDto, Employee>(MemberList.None);
            CreateMap<Employee, GetEmployeeDto>(MemberList.None);

            CreateMap<NewHolidayDto, Holiday>(MemberList.None);
            CreateMap<Holiday, NewHolidayDto>(MemberList.None);

            CreateMap<GetClientDto, Client>(MemberList.None);
            CreateMap<Client, GetClientDto>(MemberList.None);

            CreateMap<GetHolidayDto, Holiday>(MemberList.None);
            CreateMap<Holiday, GetHolidayDto>(MemberList.None);

            CreateMap<UpdateHolidayDto, Holiday>(MemberList.None);
            CreateMap<Holiday, UpdateHolidayDto>(MemberList.None);

            CreateMap<UpdateHolidayDto, GetHolidayDto>(MemberList.None);
            CreateMap<GetHolidayDto, UpdateHolidayDto>(MemberList.None);

            CreateMap<UpdateEmployeeDto, Employee>(MemberList.None);
            CreateMap<Employee, UpdateEmployeeDto>(MemberList.None);

            CreateMap<NewEmailTemplateDto, EmailTemplate>(MemberList.None);
            CreateMap<EmailTemplate, NewEmailTemplateDto>(MemberList.None);

            CreateMap<GetEmailTemplateDto, EmailTemplate>(MemberList.None);
            CreateMap<EmailTemplate, GetEmailTemplateDto>(MemberList.None);

            CreateMap<GetInventoryItemDto, InventoryItem>(MemberList.None);
            CreateMap<InventoryItem, GetInventoryItemDto>(MemberList.None);

            CreateMap<NewInventoryItemDto, InventoryItem>(MemberList.None);
            CreateMap<InventoryItem, NewInventoryItemDto>(MemberList.None);

            CreateMap<UpdateInventoryItemDto, InventoryItem>(MemberList.None);
            CreateMap<InventoryItem, UpdateInventoryItemDto>(MemberList.None);

            CreateMap<GetInventoryCategoryDto, InventoryCategory>(MemberList.None);
            CreateMap<InventoryCategory, GetInventoryCategoryDto>(MemberList.None);
        }
    }
}