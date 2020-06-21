using AutoMapper;
using XplicityApp.Dtos.Clients;
using XplicityApp.Dtos.EmailTemplates;
using XplicityApp.Dtos.Employees;
using XplicityApp.Dtos.Holidays;
using XplicityApp.Dtos.Inventory;
using XplicityApp.Dtos.NotificationSettings;
using XplicityApp.Dtos.Tags;
using XplicityApp.Dtos.Surveys;
using XplicityApp.Dtos.Surveys.Questions.Choices;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Dtos.Surveys.Questions;
using XplicityApp.Dtos.Surveys.Questions.Answers;

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
            CreateMap<Holiday, GetHolidayDto>(MemberList.None).ForMember(d => d.EmployeeFullName, opt => opt.MapFrom(h => $"{h.Employee.Name} {h.Employee.Surname}"));

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

            CreateMap<TagDto, Tag>(MemberList.None);
            CreateMap<Tag, TagDto>(MemberList.None);

            CreateMap<NewTagDto, Tag>(MemberList.None);
            CreateMap<Tag, NewTagDto>(MemberList.None);

            CreateMap<NotificationSettingsDto, NotificationSettings>(MemberList.None);
            CreateMap<NotificationSettings, NotificationSettingsDto>(MemberList.None);

            CreateMap<NewSurveyDto, Survey>(MemberList.None);
            CreateMap<Survey, NewSurveyDto>(MemberList.None);

            CreateMap<GetSurveyDto, Survey>(MemberList.None);
            CreateMap<Survey, GetSurveyDto>(MemberList.None);

            CreateMap<UpdateSurveyDto, Survey>(MemberList.None);
            CreateMap<Survey, UpdateSurveyDto>(MemberList.None);

            CreateMap<NewQuestionDto, Question>(MemberList.None);
            CreateMap<Question, NewQuestionDto>(MemberList.None);

            CreateMap<NewChoiceDto, Choice>(MemberList.None);
            CreateMap<Choice, NewChoiceDto>(MemberList.None);

            CreateMap<AnswerDto, Answer>(MemberList.None);
            CreateMap<Answer, AnswerDto>(MemberList.None);
        }
    }
}
