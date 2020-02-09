using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.IO;
using XplicityApp.Configurations;
using XplicityApp.Dtos.EmailTemplates;
using XplicityApp.Dtos.Employees;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;


namespace Tests
{
    class SetUp
    {
        private Employee[] _employees;
        private Client[] _clients;
        private Holiday[] _holidays;
        private User[] _users;
        private EmailTemplate[] _emailTemplates;
        private IdentityRole[] _roles;
        private InventoryItem[] _inventoryItems;
        private InventoryCategory[] _inventoryCategories;
        private Tag[] _tags;

        private HolidayDbContext _context;
        public HolidayDbContext HolidayDbContext =>
            _context ??
            throw new InvalidOperationException("Run initialize method before accessing this property.");

        private IMapper _mapper;
        public IMapper Mapper =>
            _mapper ??
            throw new InvalidOperationException("Run initialize method before accessing this property.");

        public void Initialize()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<HolidayDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Database")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            var configuration = GetConfiguration();
            _context = new HolidayDbContext(options, configuration);
            Seed(_context);

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperConfiguration());
            });
            _mapper = config.CreateMapper();

            //return new Tuple<HolidayDbContext, IMapper>(_context, _mapper);
        }

        public IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("testSettings.json")
                          .Build();
            return config;
        }

        public UserManager<User> InitializeUserManager()
        {
            var userStore = new UserStore<User>(_context);

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

        public RoleManager<IdentityRole> InitializeRoleManager()
        {
            var roleStore = new RoleStore<IdentityRole>(_context);

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
            _roles = new[]
            {
                new IdentityRole
                {
                    Name = "Employee",
                    NormalizedName = "Employee",
                },
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                },
            };
            context.Roles.AddRange(_roles);

            _emailTemplates = new[]
            {
                new EmailTemplate
                {
                    Purpose = "purpose1",
                    Subject = "subject1",
                    Template = "template1",
                    Instructions = "instructions1"
                },
                new EmailTemplate
                {
                    Purpose = "purpose2",
                    Subject = "subject2",
                    Template = "template2",
                    Instructions = "instructions2"
                },
            };
            context.EmailTemplates.AddRange(_emailTemplates);

            _employees = new[] {
                new Employee
                {
                    ClientId = 1,
                    Name = "EmployeeName1",
                    Surname = "EmployeeSurname1",
                    Email = "taken1@email",
                    WorksFromDate = new DateTime(2019,02,25),
                    DaysOfVacation = 20,
                    BirthdayDate = new DateTime(1988,09,12),
                    FreeWorkDays = 10,
                    OvertimeHours = 24,
                    ParentalLeaveLimit = 3,
                    CurrentAvailableLeaves = 1,
                    NextMonthAvailableLeaves = 2,
                },
                new Employee
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

            _clients = new[] {
                new Client
                {
                    CompanyName = "CompanyName1",
                    OwnerName = "OwnerName1",
                    OwnerSurname = "OwnerSurname1",
                    OwnerEmail = "e1@gmail.com",
                    OwnerPhone = "111"
                },
                new Client
                {
                    CompanyName = "CompanyName2",
                    OwnerName = "OwnerName2",
                    OwnerSurname = "OwnerSurname2",
                    OwnerEmail = "e2@gmail.com",
                    OwnerPhone = "222"
                },
            };
            context.Clients.AddRange(_clients);

            _holidays = new[] {
                new Holiday
                {
                    Employee = context.Employees.Find(1),
                    EmployeeId = 1,
                    Type = HolidayType.Parental,
                    FromInclusive = DateTime.Today.AddDays(1),
                    ToInclusive = DateTime.Today.AddDays(14),
                    Status = HolidayStatus.Pending,
                    RequestCreatedDate = new DateTime(2019, 10, 13),
                    Paid = false
                },
                new Holiday
                {
                    Employee = context.Employees.Find(2),
                    EmployeeId = 2,
                    Type = HolidayType.Annual,
                    FromInclusive = DateTime.Today.AddDays(-1),
                    ToInclusive = DateTime.Today.AddDays(6),
                    Status = HolidayStatus.Confirmed,
                    RequestCreatedDate = new DateTime(2019, 10, 14),
                    Paid = false
                },
                new Holiday
                {
                    Employee = context.Employees.Find(1),
                    EmployeeId = 1,
                    Type = HolidayType.Annual,
                    FromInclusive = DateTime.Today.AddDays(1),
                    ToInclusive = DateTime.Today.AddDays(6),
                    Status = HolidayStatus.Confirmed,
                    OvertimeDays = 3,
                    RequestCreatedDate = new DateTime(2019, 10, 14),
                    Paid = true
                },
            };
            context.Holidays.AddRange(_holidays);

            _users = new[] {
                new User
                {
                    Employee = context.Employees.Find(1),
                    EmployeeId = 1,
                    Email = "userEmail",
                    UserName = "userName",
                    NormalizedEmail = "userEmail"
                }
            };
            context.Users.AddRange(_users);

            _inventoryCategories = new[]
            {
                new InventoryCategory
                {
                    Name = "Category1",
                    Deprecation = 1
                },
                new InventoryCategory
                {
                    Name = "Category2",
                    Deprecation = 2
                }
            };
            context.InventoryCategories.AddRange(_inventoryCategories);

            _inventoryItems = new[]
            {
                new InventoryItem
                {
                    Name = "Item1",
                    SerialNumber = "Serial no 1",
                    PurchaseDate = DateTime.Today,
                    ExpiryDate = null,
                    Price = 100,
                    Comment = null,
                    Category = context.InventoryCategories.Find(1),
                    InventoryCategoryId = 1
                },
                new InventoryItem
                {
                    Name = "Item2",
                    SerialNumber = "Serial no 2",
                    PurchaseDate = DateTime.Today,
                    ExpiryDate = null,
                    Price = 100,
                    Comment = null,
                    Category = context.InventoryCategories.Find(2),
                    InventoryCategoryId = 2
                },
            };
            context.InventoryItems.AddRange(_inventoryItems);

            _tags = new[]
            {
                new Tag
                {
                    Id = 1,
                    Title = "Tag1"
                },
                  new Tag
                {
                    Id = 2,
                    Title = "Tag2"
                },
            };
            context.Tags.AddRange(_tags);

            context.SaveChanges();
        }

        public int GetCount(string type)
        {
            switch (type)
            {
                case "employees":
                    return _employees.Length;
                case "clients":
                    return _clients.Length;
                case "holidays":
                    return _holidays.Length;
                case "users":
                    return _users.Length;
                case "emailTemplates":
                    return _emailTemplates.Length;
                case "roles":
                    return _roles.Length;
                case "inventoryItems":
                    return _inventoryItems.Length;
                case "inventoryCategories":
                    return _inventoryCategories.Length;
                case "tags":
                    return _tags.Length;
                default:
                    return 0;
            }
        }

        public static NewEmployeeDto NewEmployeeDto(int clientId, string password, string email)
        {
            var newEmployeeDto = new NewEmployeeDto
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

        //public Employee NewEmployee(int clientId, string email)
        //{
        //    var newEmployee = new Employee
        //    {
        //        ClientId = clientId,
        //        Email = email,

        //        Name = "EmployeeNameNew",
        //        Surname = "EmployeeSurnameNew",
        //        WorksFromDate = new DateTime(2019, 07, 06),
        //        DaysOfVacation = 20,
        //        BirthdayDate = new DateTime(1988, 07, 06),
        //        ParentalLeaveLimit = 30,
        //        Position = "Position"
        //    };

        //    return newEmployee;
        //}

        //public NewHolidayDto NewHolidayDto()
        //{
        //    var newHolidayDto = new NewHolidayDto
        //    {
        //        EmployeeId = 1,
        //        Type = HolidayType.Parental,
        //        FromInclusive = new DateTime(2019, 11, 11),
        //        ToExclusive = new DateTime(2019, 11, 18),
        //        Paid = true
        //    };

        //    return newHolidayDto;
        //}

        public static NewEmailTemplateDto NewEmailTemplateDto()
        {
            var newEmailTemplateDto = new NewEmailTemplateDto
            {
                Purpose = "new purpose",
                Subject = "new subject",
                Template = "new template",
                Instructions = "new instructions"
            };

            return newEmailTemplateDto;
        }

        //public User NewUser()
        //{
        //    var newUser = new User
        //    {
        //        EmployeeId = 1,
        //        Email = "emailNew",
        //        UserName = "usrnameNew",
        //        NormalizedEmail = "emailNew"
        //    };

        //    return newUser;
        //}
    }
}
