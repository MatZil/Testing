using AutoMapper;
using Xplicity_Holidays.Dtos.Clients;
using Xplicity_Holidays.Dtos.Employees;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Configurations
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration() : this("Holidays")
        {

        }
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
        }
            
    }
}
