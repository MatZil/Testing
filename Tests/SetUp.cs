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
using XplicityApp.Infrastructure.Static_Files;
using XplicityApp.Infrastructure.Utils;

namespace Tests
{
    class SetUp
    {
        private Employee[] _employees;
        private Client[] _clients;
        private Holiday[] _holidays;
        private User[] _users;
        private EmailTemplate[] _emailTemplates;
        private InventoryItem[] _inventoryItems;
        private InventoryCategory[] _inventoryCategories;
        private Tag[] _tags;
        private InventoryItemTag[] _inventoryItemTags;
        private FileRecord[] _fileRecords;
        private NotificationSettings[] _notificationSettings;
        private TimeService _timeService = new TimeService();

        private HolidayDbContext _context;
        public HolidayDbContext HolidayDbContext =>
            _context ??
            throw new InvalidOperationException("Run initialize method before accessing this property.");

        private IMapper _mapper;
        public IMapper Mapper =>
            _mapper ??
            throw new InvalidOperationException("Run initialize method before accessing this property.");

        private UserManager<User> _userManager;
        public UserManager<User> UserManager =>
            _userManager ??
            throw new InvalidOperationException("Run initialize method before accessing this property.");

        private RoleManager<IdentityRole> _roleManager;
        public RoleManager<IdentityRole> RoleManager =>
            _roleManager ??
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
        }

        public IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json")
                          .AddJsonFile("email-templates-settings.json")
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
                null,
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
                null,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

            return roleManager;
        }

        private async void Seed(HolidayDbContext context)
        {
            var config = GetConfiguration();

            var userStore = new UserStore<User>(context);
            _userManager = InitializeUserManager();

            var roleStore = new RoleStore<IdentityRole>(context);
            _roleManager = InitializeRoleManager();

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!context.Users.AnyAsync(x => x.UserName == "user1").Result)
            {
                var user = new User { UserName = "user1", Email = "user1@gmail.com", EmployeeId = 1 };
                await _userManager.CreateAsync(user, "Pa$$W0rD!");

                if (!_userManager.IsInRoleAsync(user, "Admin").Result)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }

            _emailTemplates = new[]
            {
                new EmailTemplate
                    {
                        Id = 1,
                        Purpose = EmailPurposes.ADMIN_CONFIRMATION,
                        Subject = config["EmailTemplates:AdminConfirmation:Subject"],
                        Template = config["EmailTemplates:AdminConfirmation:Template"],
                        Instructions = config["EmailTemplates:AdminConfirmation:Instructions"]
                    },
                    new EmailTemplate
                    {
                        Id = 2,
                        Purpose = EmailPurposes.CLIENT_CONFIRMATION,
                        Subject = config["EmailTemplates:ClientConfirmation:Subject"],
                        Template = config["EmailTemplates:ClientConfirmation:Template"],
                        Instructions = config["EmailTemplates:ClientConfirmation:Instructions"]
                    },

                    new EmailTemplate
                    {
                        Id = 3,
                        Purpose = EmailPurposes.MONTHLY_HOLIDAYS_REPORT,
                        Subject = config["EmailTemplates:MonthlyReport:Subject"],
                        Template = config["EmailTemplates:MonthlyReport:Template"],
                        Instructions = config["EmailTemplates:MonthlyReport:Instructions"]
                    },
                    new EmailTemplate
                    {
                        Id = 4,
                        Purpose = EmailPurposes.HOLIDAY_REMINDER,
                        Subject = config["EmailTemplates:HolidayNotification:Subject"],
                        Template = config["EmailTemplates:HolidayNotification:Template"],
                        Instructions = config["EmailTemplates:HolidayNotification:Instructions"]
                    },
                    new EmailTemplate
                    {
                        Id = 5,
                        Purpose = EmailPurposes.BIRTHDAY_REMINDER,
                        Subject = config["EmailTemplates:BirthdayReminder:Subject"],
                        Template = config["EmailTemplates:BirthdayReminder:Template"],
                        Instructions = config["EmailTemplates:BirthdayReminder:Instructions"]
                    },
                    new EmailTemplate
                    {
                        Id = 6,
                        Purpose = EmailPurposes.REQUEST_NOTIFICATION,
                        Subject = config["EmailTemplates:RequestNotification:Subject"],
                        Template = config["EmailTemplates:RequestNotification:Template"],
                        Instructions = config["EmailTemplates:RequestNotification:Instructions"]
                    },
                    new EmailTemplate
                    {
                        Id = 7,
                        Purpose = EmailPurposes.ORDER_NOTIFICATION,
                        Subject = config["EmailTemplates:OrderNotification:Subject"],
                        Template = config["EmailTemplates:OrderNotification:Template"],
                        Instructions = config["EmailTemplates:OrderNotification:Instructions"]
                    }
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
                    BirthdayDate = DateTime.Today,
                    FreeWorkDays = 10,
                    OvertimeHours = 24,
                    ParentalLeaveLimit = 3,
                    CurrentAvailableLeaves = 1,
                    NextMonthAvailableLeaves = 2,
                },
                new Employee
                {
                    ClientId = 2,
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
                new Employee
                {
                    Name = "EmployeeName3",
                    Surname = "EmployeeSurname3",
                    Email = "taken3@email",
                    WorksFromDate = new DateTime(2019,01,06),
                    DaysOfVacation = 20,
                    BirthdayDate = new DateTime(1987,07,06),
                    FreeWorkDays = 15,
                    OvertimeHours = 24,
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
                    Status = HolidayStatus.AdminConfirmed,
                    RequestCreatedDate = new DateTime(2019, 10, 14),
                    Paid = false
                },
                new Holiday
                {
                    Employee = context.Employees.Find(3),
                    EmployeeId = 3,
                    Type = HolidayType.Annual,
                    FromInclusive = _timeService.GetNextWorkDay(DateTime.Today),
                    ToInclusive = _timeService.GetNextWorkDay(DateTime.Today).AddDays(5),
                    Status = HolidayStatus.AdminConfirmed,
                    OvertimeDays = 3,
                    RequestCreatedDate = new DateTime(2019, 10, 14),
                    Paid = true
                },
                new Holiday //4
                {
                    Employee = context.Employees.Find(3),
                    EmployeeId = 3,
                    Type = HolidayType.Annual,
                    FromInclusive = new DateTime(2020, 02, 13),
                    ToInclusive = new DateTime(2020, 02, 18),
                    Status = HolidayStatus.Pending,
                    OvertimeDays = 4,
                    RequestCreatedDate = new DateTime(2019, 12, 14),
                    Paid = true
                },
                new Holiday //5
                {
                    Employee = context.Employees.Find(3),
                    EmployeeId = 3,
                    Type = HolidayType.Annual,
                    FromInclusive = new DateTime(2020, 02, 17),
                    ToInclusive = new DateTime(2020, 02, 19),
                    Status = HolidayStatus.Pending,
                    OvertimeDays = 3,
                    RequestCreatedDate = new DateTime(2019, 12, 14),
                    Paid = true
                },
                new Holiday //6
                {
                    Employee = context.Employees.Find(3),
                    EmployeeId = 3,
                    Type = HolidayType.Annual,
                    FromInclusive = new DateTime(2020, 02, 24),
                    ToInclusive = new DateTime(2020, 02, 28),
                    Status = HolidayStatus.Pending,
                    OvertimeDays = 2,
                    RequestCreatedDate = new DateTime(2019, 12, 14),
                    Paid = true
                },
                new Holiday //7
                {
                    Employee = context.Employees.Find(3),
                    EmployeeId = 3,
                    Type = HolidayType.Annual,
                    FromInclusive = new DateTime(2020, 02, 13),
                    ToInclusive = new DateTime(2020, 02, 20),
                    Status = HolidayStatus.Pending,
                    OvertimeDays = 2,
                    RequestCreatedDate = new DateTime(2019, 12, 14),
                    Paid = true
                },
                new Holiday //8
                {
                    Employee = context.Employees.Find(3),
                    EmployeeId = 3,
                    Type = HolidayType.Annual,
                    FromInclusive = new DateTime(2020, 02, 13),
                    ToInclusive = new DateTime(2020, 02, 20),
                    Status = HolidayStatus.Pending,
                    OvertimeDays = 0,
                    RequestCreatedDate = new DateTime(2019, 12, 14),
                    Paid = true
                },
                new Holiday //9
                {
                    Employee = context.Employees.Find(3),
                    EmployeeId = 3,
                    Type = HolidayType.Annual,
                    FromInclusive = new DateTime(2020, 02, 19),
                    ToInclusive = new DateTime(2020, 02, 25),
                    Status = HolidayStatus.Pending,
                    OvertimeDays = 0,
                    RequestCreatedDate = new DateTime(2019, 12, 14),
                    Paid = true
                },
                new Holiday //10 for validation fail 10-14
                {
                    Employee = context.Employees.Find(3),
                    EmployeeId = 3,
                    Type = HolidayType.Annual,
                    FromInclusive = new DateTime(2021, 02, 19),
                    ToInclusive = new DateTime(2021, 02, 18),
                    Status = HolidayStatus.Pending,
                    OvertimeDays = 0,
                    RequestCreatedDate = new DateTime(2019, 12, 14),
                    Paid = true
                },
                new Holiday //11
                {
                    Employee = context.Employees.Find(3),
                    EmployeeId = 3,
                    Type = HolidayType.Annual,
                    FromInclusive = new DateTime(2021, 02, 19),
                    ToInclusive = new DateTime(2021, 02, 25),
                    Status = HolidayStatus.Pending,
                    OvertimeDays = 100,
                    RequestCreatedDate = new DateTime(2019, 12, 14),
                    Paid = true
                },
                new Holiday //12
                {
                    Employee = context.Employees.Find(3),
                    EmployeeId = 1,
                    Type = HolidayType.Annual,
                    FromInclusive = new DateTime(2021, 02, 19),
                    ToInclusive = new DateTime(2021, 02, 20),
                    Status = HolidayStatus.Pending,
                    OvertimeDays = 3,
                    RequestCreatedDate = new DateTime(2019, 12, 14),
                    Paid = true
                },
                new Holiday //13
                {
                    Employee = context.Employees.Find(3),
                    EmployeeId = 1,
                    Type = HolidayType.Parental,
                    FromInclusive = new DateTime(2021, 02, 19),
                    ToInclusive = new DateTime(2021, 02, 20),
                    Status = HolidayStatus.Pending,
                    OvertimeDays = 3,
                    RequestCreatedDate = new DateTime(2019, 12, 14),
                    Paid = true
                },
                new Holiday //14
                {
                    Employee = context.Employees.Find(3),
                    EmployeeId = 1,
                    Type = HolidayType.Annual,
                    FromInclusive = new DateTime(2021, 02, 20),
                    ToInclusive = new DateTime(2021, 02, 24),
                    Status = HolidayStatus.Pending,
                    OvertimeDays = 4,
                    RequestCreatedDate = new DateTime(2019, 12, 14),
                    Paid = true
                },
                new Holiday //15
                {
                    Employee = context.Employees.Find(3),
                    EmployeeId = 3,
                    Type = HolidayType.Annual,
                    FromInclusive = new DateTime(2020, 12, 16),
                    ToInclusive = new DateTime(2020, 12, 21),
                    Status = HolidayStatus.Pending,
                    OvertimeDays = 4,
                    RequestCreatedDate = new DateTime(2019, 12, 14),
                    Paid = true
                },
                new Holiday //16
                {
                    Employee = context.Employees.Find(3),
                    EmployeeId = 3,
                    Type = HolidayType.Annual,
                    FromInclusive = new DateTime(2020, 12, 16),
                    ToInclusive = new DateTime(2020, 12, 18),
                    Status = HolidayStatus.Pending,
                    OvertimeDays = 3,
                    RequestCreatedDate = new DateTime(2019, 12, 14),
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
                    new Tag
                {
                    Id = 3,
                    Title = "No3"
                },
                    new Tag
                    {
                    Id = 4,
                    Title = "No4"
                },
            };
            context.Tags.AddRange(_tags);

            _fileRecords = new[]
            {
                new FileRecord
                {
                    Id = 1,
                    Name = "Order",
                    Type = FileTypeEnum.Order,
                    CreatedAt = DateTime.Today
                },
                new FileRecord
                {
                    Id = 2,
                    Name = "Request",
                    Type = FileTypeEnum.Request,
                    CreatedAt = DateTime.Today
                }
            };
            context.FileRecords.AddRange(_fileRecords);

            _inventoryItemTags = new[]
                {
                new InventoryItemTag
                {
                    Id = 1,
                    InventoryItemId = 2,
                    TagId = 1
                },
                new InventoryItemTag
                {
                     Id = 1,
                    InventoryItemId = 2,
                    TagId = 4
                },
            };
            context.InventoryItemsTags.AddRange(_inventoryItemTags);
            context.SaveChanges();

            _notificationSettings = new[]
            {
                new NotificationSettings
                {
                    Id = 1,
                    EmployeeId = 1
                }
            };
            context.NotificationSettings.AddRange(_notificationSettings);
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
                    return _roleManager.Roles.ToListAsync().Result.Count;
                case "inventoryItems":
                    return _inventoryItems.Length;
                case "inventoryCategories":
                    return _inventoryCategories.Length;
                case "tags":
                    return _tags.Length;
                case "notificationSettings":
                    return _notificationSettings.Length;
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
