using AutoMapper;
using Xplicity_Holidays.Dtos;
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
            CreateMap<NewClientDto, Client>();
            CreateMap<NewEmployeeDto, Employee>();
            CreateMap<Client, NewClientDto>();
        }
            
    }
}
