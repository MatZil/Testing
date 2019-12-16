using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services;
using Xunit;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Utils;
using AutoMapper;
using Xplicity_Holidays.Services.Interfaces;
using Moq;
using Xplicity_Holidays.Dtos.Holidays;
using Xunit.Abstractions;

namespace Tests
{
    [TestCaseOrderer("Tests.HolidayConfirmTests.AlphabeticalOrderer", "Tests")]
    public class HolidayConfirmTests
    {
        private readonly HolidayDbContext _context;
        private readonly HolidayConfirmService _holidayConfirmService;
        private readonly HolidaysService _holidaysService;
        private readonly ITestOutputHelper _output;
        private readonly HolidaysRepository _holidaysRepository;
        private readonly IMapper _mapper;
        private readonly EmployeesRepository _employeesRepository;
        private readonly TimeService _timeService;

        public HolidayConfirmTests(ITestOutputHelper output)
        {
            _output = output;
            var setup = new SetUp();
            var contextMapperTuple = setup.Initialize();
            _context = contextMapperTuple.Item1;
            var mapper = contextMapperTuple.Item2;
            _mapper = mapper;

            _timeService = new TimeService();
            _holidaysRepository = new HolidaysRepository(_context);
            var userManager = setup.InitializeUserManager(_context);
            _employeesRepository = new EmployeesRepository(_context, userManager);
            IRepository<Client> clientsRepository = new ClientsRepository(_context);
            var emailService = new Mock<IEmailService>();
            var docxGeneratorService = new Mock<IDocxGeneratorService>();

            _holidaysService = new HolidaysService(_holidaysRepository, mapper, _timeService);
            _holidayConfirmService = new HolidayConfirmService(emailService.Object, mapper, _holidaysRepository,
                                                                _employeesRepository, clientsRepository, _holidaysService, _timeService, docxGeneratorService.Object);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_RequestingClientApproval_Expect_True(int holidayId)
        {
            var result = await _holidayConfirmService.RequestClientApproval(holidayId);

            Assert.True(result, "Request for client's approval was successfully submitted.");
        }

        //[Theory]
        //[InlineData(1, "status")]
        //public async void When_RequestingAdminApproval_Expect_True(int holidayId, string clientStatus)
        //{
        //    var result = await _holidayConfirmService.RequestAdminApproval(holidayId, clientStatus);
        //    Assert.True(result);
        //}

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_ConfirmingHoliday_Expect_True(int holidayId)
        {
            var holiday = await _holidaysRepository.GetById(holidayId);

            await _holidayConfirmService.ConfirmHoliday(holidayId);

            var updatedHoliday = await _holidaysRepository.GetById(holidayId);
            var status = updatedHoliday.Status.ToString();

            Assert.True(status == "Confirmed", "Failed to confirm holiday.");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void When_UpdatingEmployeesWorkdays_Expect_UpdatesWorkdays(int holidayId)
        {
            var index = _context.Holidays.Find(holidayId).EmployeeId;
            var employee = await _employeesRepository.GetById(index);
            var initial = employee.FreeWorkDays;
            var holiday = await _holidaysRepository.GetById(holidayId);
            var holidayDto = _mapper.Map<GetHolidayDto>(holiday);

            var workdays = _timeService.GetWorkDays(holidayDto.FromInclusive, holidayDto.ToExclusive);
            var expected = initial - workdays + holiday.OvertimeDays;

            _holidayConfirmService.call("UpdateEmployeesWorkdays", holidayDto);

            employee = await _employeesRepository.GetById(index);
            var actual = employee.FreeWorkDays;

            Assert.True(expected == actual, "Failed to update employee's free workdays.");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public async void When_UpdatingEmployeesOvertime_Expect_UpdatesOvertime(int holidayId)
        {
            var index = _context.Holidays.Find(holidayId).EmployeeId;
            var employee = await _employeesRepository.GetById(index);
            var initial = employee.OvertimeHours;
            var holiday = await _holidaysRepository.GetById(holidayId);
            var holidayDto = _mapper.Map<GetHolidayDto>(holiday);

            var expected = initial - holiday.OvertimeHours;

            _holidayConfirmService.call("UpdateEmployeesOvertime", holidayDto);

            employee = await _employeesRepository.GetById(index);
            var actual = employee.OvertimeHours;

            Assert.True(expected == actual, "Failed to update employee's overtime hours.");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_UpdatingParentalLeaves_Expect_UpdatesParentalLeaves(int holidayId)
        {
            var holiday = await _holidaysRepository.GetById(holidayId);
            var holidayDto = _mapper.Map<GetHolidayDto>((Holiday)holiday);

            var employee = await _employeesRepository.GetById(holidayDto.EmployeeId);
            var initial = new int[2];
            initial[0] = employee.CurrentAvailableLeaves;
            initial[1] = employee.NextMonthAvailableLeaves;

            _holidayConfirmService.call("UpdateParentalLeaves", holidayDto);

            var final = new int[2];
            final[0] = employee.CurrentAvailableLeaves;
            final[1] = employee.NextMonthAvailableLeaves;

            Assert.True(initial[0] != final[0] || initial[1] != final[1], "Failed to update employee's parental leaves.");
        }

        //[Theory]
        //[InlineData(1)]
        //public async void When_ConfirmingValid_Expect_True(int holidayId)
        //{
        //    var result = await _holidayConfirmService.IsValid(holidayId);

        //    Assert.True(result, "Holiday request is deemed to be invalid.");
        //}

        [Theory]
        [InlineData(4)]
        public async void When_ConfirmingInvalid_Expect_False(int holidayId)
        {
            var result = await _holidayConfirmService.IsValid(holidayId);

            Assert.False(result, "Holiday request is deemed to be valid.");
        }
    }
}
