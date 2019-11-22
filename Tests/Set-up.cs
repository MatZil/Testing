using System;
using AutoMapper;
using Xplicity_Holidays.Dtos.Employees;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Dtos.EmailTemplates;
using Xplicity_Holidays.Infrastructure.Enums;
using System.IO;
using Microsoft.Extensions.Configuration.Json;


namespace Tests
{
    class Set_up
    {
        private Employee[] _employees;
        private Client[] _clients;
        private Holiday[] _holidays;
        private User[] _users;
        private EmailTemplate[] _emailTemplates;
        private IdentityRole[] _roles;

        public void Initialize(out HolidayDbContext context, out IMapper mapper)
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<HolidayDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Database")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            var builder = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("testSettings.json");
            var configuration = builder.Build();


            context = new HolidayDbContext(options, configuration);
            Seed(context);

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperConfiguration());
            });
            mapper = config.CreateMapper();
        }

        public UserManager<User> InitializeUserManager(HolidayDbContext context)
        {
            var userStore = new UserStore<User>(context);

            var userManager = new UserManager<User>(
                userStore,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);

            return userManager;
        }

        public RoleManager<IdentityRole> InitializeRoleManager(HolidayDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);

            var roleManager = new RoleManager<IdentityRole>(
                roleStore,
                new IRoleValidator<IdentityRole>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

            return roleManager;
        }

        private void Seed(HolidayDbContext context)
        {
            _roles = new IdentityRole[]
            {
                new IdentityRole()
                {
                    Name = "Employee",
                    NormalizedName = "Employee",
                },
                new IdentityRole()
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                },
            };
            context.Roles.AddRange(_roles);

            _emailTemplates = new EmailTemplate[]
            {
                new EmailTemplate()
                {
                    Purpose = "purpose1",
                    Subject = "subject1",
                    Template = "template1",
                    Instructions = "instructions1"
                },
                new EmailTemplate()
                {
                    Purpose = "purpose2",
                    Subject = "subject2",
                    Template = "template2",
                    Instructions = "instructions2"
                },
            };
            context.EmailTemplates.AddRange(_emailTemplates);

            _employees = new Employee[] {
                new Employee()
                {
                    ClientId = 1,
                    Name = "EmployeeName1",
                    Surname = "EmployeeSurname1",
                    Email = "taken1@email",
                    WorksFromDate = new DateTime(2019,02,25),
                    DaysOfVacation = 20,
                    BirthdayDate = new DateTime(1988,09,12),
                    FreeWorkDays = 10,
                    ParentalLeaveLimit = 3,
                    CurrentAvailableLeaves = 1,
                    NextMonthAvailableLeaves = 2,
                },
                new Employee()
                {
                    ClientId = 1,
                    Client = context.Clients.Find(1),
                    Name = "EmployeeName2",
                    Surname = "EmployeeSurname2",
                    Email = "taken2@email",
                    WorksFromDate = new DateTime(2018,01,06),
                    DaysOfVacation = 20,
                    BirthdayDate = new DateTime(1988,07,06),
                    FreeWorkDays = 15,
                    ParentalLeaveLimit = 4,
                    CurrentAvailableLeaves = 2,
                    NextMonthAvailableLeaves = 1,
                },
            };
            context.Employees.AddRange(_employees);

            _clients = new Client[] {
                new Client()
                {
                    CompanyName = "CompanyName1",
                    OwnerName = "OwnerName1",
                    OwnerSurname = "OwnerSurname1",
                    OwnerEmail = "e1@gmail.com",
                    OwnerPhone = "111"
                },
                new Client()
                {
                    CompanyName = "CompanyName2",
                    OwnerName = "OwnerName2",
                    OwnerSurname = "OwnerSurname2",
                    OwnerEmail = "e2@gmail.com",
                    OwnerPhone = "222"
                },
            };
            context.Clients.AddRange(_clients);

            _holidays = new Holiday[] {
                new Holiday()
                {
                    Employee = context.Employees.Find(1),
                    EmployeeId = 1,
                    Type = HolidayType.Parental,
                    FromInclusive = new DateTime(2019, 10, 01),
                    ToExclusive = new DateTime(2019, 10, 14),
                    Status = HolidayStatus.Unconfirmed,
                    RequestCreatedDate = new DateTime(2019, 10, 13),
                },
                new Holiday()
                {
                    Employee = context.Employees.Find(2),
                    EmployeeId = 2,
                    Type = HolidayType.Annual,
                    FromInclusive = new DateTime(2019,12,05),
                    ToExclusive = new DateTime(2019,12,11),
                    Status = HolidayStatus.Confirmed,
                    RequestCreatedDate = new DateTime(2019, 10, 14),
                },
            };
            context.Holidays.AddRange(_holidays);

            _users = new User[] {
                new User()
                {
                    Employee = context.Employees.Find(1),
                    EmployeeId = 1,
                    Email = "userEmail",
                    UserName = "userName",
                    NormalizedEmail = "userEmail"
                }
            };
            context.Users.AddRange(_users);

            context.SaveChanges();
        }

        public int GetCount(string type)
        {
            if (type == "employees")
                return _employees.Length;
            else if (type == "clients")
                return _clients.Length;
            else if (type == "holidays")
                return _holidays.Length;
            else if (type == "users")
                return _users.Length;
            else if (type == "emailTemplates")
                return _emailTemplates.Length;
            else if (type == "roles")
                return _roles.Length;

            return 0;
        }

        public NewEmployeeDto NewEmployeeDto(int clientId, string password, string email)
        {
            var newEmployeeDto = new NewEmployeeDto()
            {
                ClientId = clientId,
                Email = email,
                Password = password,

                Name = "EmployeeNameNew",
                Surname = "EmployeeSurnameNew",
                WorksFromDate = new DateTime(2019, 07, 06),
                DaysOfVacation = 20,
                BirthdayDate = new DateTime(1988, 07, 06),
                ParentalLeaveLimit = 30,
                Role = "Employee",
                Position = "Position"
            };

            return newEmployeeDto;
        }

        public Employee NewEmployee(int clientId, string email)
        {
            var newEmployee = new Employee()
            {
                ClientId = clientId,
                Email = email,

                Name = "EmployeeNameNew",
                Surname = "EmployeeSurnameNew",
                WorksFromDate = new DateTime(2019, 07, 06),
                DaysOfVacation = 20,
                BirthdayDate = new DateTime(1988, 07, 06),
                ParentalLeaveLimit = 30,
                Position = "Position"
            };

            return newEmployee;
        }

        public NewHolidayDto NewHolidayDto()
        {
            var newHolidayDto = new NewHolidayDto()
            {
                EmployeeId = 1,
                Type = HolidayType.Parental,
                FromInclusive = new DateTime(2019, 11, 11),
                ToExclusive = new DateTime(2019, 11, 18),
                Paid = true
            };

            return newHolidayDto;
        }

        public NewEmailTemplateDto NewEmailTemplateDto()
        {
            var newEmailTemplateDto = new NewEmailTemplateDto()
            {
                Purpose = "new purpose",
                Subject = "new subject",
                Template = "new template",
                Instructions = "new instructions"
            };

            return newEmailTemplateDto;
        }

        public User NewUser()
        {
            var newUser = new User()
            {
                EmployeeId = 1,
                Email = "emailNew",
                UserName = "usrnameNew",
                NormalizedEmail = "emailNew"
            };

            return newUser;
        }
    }
}
