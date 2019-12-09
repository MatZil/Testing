using AutoMapper;
using Xplicity_Holidays.Dtos.Clients;
using Xplicity_Holidays.Dtos.EmailTemplates;
using Xplicity_Holidays.Dtos.Employees;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Dtos.Inventory;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Configurations
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

            CreateMap<GetInventoryCategoryDto, InventoryCategory>(MemberList.None);
            CreateMap<InventoryCategory, GetInventoryCategoryDto>(MemberList.None);
        }
    }
}
