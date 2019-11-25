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
        private readonly Set_up _setup;
        private readonly ITestOutputHelper _output;
        private readonly HolidaysRepository _holidaysRepository;
        private readonly IMapper _mapper;
        private readonly EmployeesRepository _employeesRepository;

        public HolidayConfirmTests(ITestOutputHelper output)
        {
            _output = output;
            _setup = new Set_up();
            _setup.Initialize(out _context, out IMapper mapper);
            _mapper = mapper;

            var timeService = new TimeService();
            _holidaysRepository = new HolidaysRepository(_context);
            var userManager = _setup.InitializeUserManager(_context);
            _employeesRepository = new EmployeesRepository(_context, userManager);
            IRepository<Client> clientsRepository = new ClientsRepository(_context);
            var pdfService = new Mock<IPdfService>();
            var emailService = new Mock<IEmailService>();

            _holidaysService = new HolidaysService(_holidaysRepository, mapper, timeService);
            _holidayConfirmService = new HolidayConfirmService(emailService.Object, mapper, _holidaysRepository,
                pdfService.Object, _employeesRepository, clientsRepository, _holidaysService, timeService);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_RequestingClientApproval_Expect_True(int holidayId)
        {
            var result = await _holidayConfirmService.RequestClientApproval(holidayId);

            Assert.True(result);
        }

        //[Theory]
        //[InlineData(1, "status")]
        //public async void When_RequestingAdminApproval_Expect_True(int holidayId, string clientStatus)
        //{
        //    var result = await _holidayConfirmService.RequestAdminApproval(holidayId, clientStatus);
        //    Assert.True(result);
        //}

        [Fact]
        public async void When_CreatingRequestPdf_Expect_CreatesRequestPdf()
        {
            NewHolidayDto holidayDto = _setup.NewHolidayDto();

            var result = await _holidayConfirmService.CreateRequestPdf(holidayDto, 3);

            Assert.True(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_CreatingOrderPdf_Expect_CreatesOrderPdf(int holidayId)
        {
            var result = await _holidayConfirmService.CreateOrderPdf(holidayId);

            Assert.True(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_ConfirmingHoliday_Expect_True(int holidayId)
        {
            var holiday = await _holidaysRepository.GetById(holidayId);
            _output.WriteLine(holiday.Status.ToString());

            await _holidayConfirmService.ConfirmHoliday(holidayId);

            var updatedHoliday = await _holidaysRepository.GetById(holidayId);
            _output.WriteLine(updatedHoliday.Status.ToString());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_UpdatingEmployeesWorkdays_Expect_UpdatesWorkdays(int holidayId)
        {
            var index = _context.Holidays.Find(holidayId).EmployeeId;
            var employee = await _employeesRepository.GetById(index);
            var initial = employee.FreeWorkDays;

            var holiday = await _holidaysRepository.GetById(holidayId);
            var holidayDto = _mapper.Map<GetHolidayDto>((Holiday)holiday);

            _holidayConfirmService.call("UpdateEmployeesWorkdays", holidayDto);

            employee = await _employeesRepository.GetById(index);
            var final = employee.FreeWorkDays;

            Assert.True(final < initial);
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

            Assert.True(initial[0] != final[0] || initial[1] != final[1]);
        }

        //[Theory]
        //[InlineData(1)]
        //public async void When_ConfirmingValid_Expect_True(int holidayId)
        //{
        //    var result = await _holidayConfirmService.IsValid(holidayId);

        //    Assert.True(result);
        //}

        [Theory]
        [InlineData(2)]
        public async void When_ConfirmingInvalid_Expect_False(int holidayId)
        {
            var result = await _holidayConfirmService.IsValid(holidayId);

            Assert.False(result);
        }
    }
}
