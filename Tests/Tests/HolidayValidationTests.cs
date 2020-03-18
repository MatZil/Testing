using AutoMapper;
using Moq;
using System;
using XplicityApp.Dtos.Holidays;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils;
using XplicityApp.Services.Validations;
using Xunit;

namespace Tests.Tests
{
    [TestCaseOrderer("Tests.HolidayValidationTests.AlphabeticalOrderer", "Tests")]
    public class HolidayValidationTests
    {
        private readonly HolidayDbContext _context;
        private readonly HolidayValidationService _holidayValidationService;
        private readonly HolidaysRepository _holidaysRepository;
        private readonly IMapper _mapper;
        private readonly EmployeesRepository _employeesRepository;
        private readonly TimeService _timeService;


        public HolidayValidationTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            _context = setup.HolidayDbContext;
            _mapper = setup.Mapper;

            _timeService = new Mock<TimeService>().Object;
            _holidaysRepository = new HolidaysRepository(_context);
            var userManager = setup.InitializeUserManager();
            _employeesRepository = new EmployeesRepository(_context, userManager);

            _holidayValidationService = new HolidayValidationService(
                _holidaysRepository,
                _employeesRepository,
                _mapper,
                _timeService);
        }
        [Theory]
        [InlineData(15, 1)]
        [InlineData(16, 1)]
        public async void When_ValidatingHolidayConfirmationReadiness_Expect_ValidationSuccess(int holidayId, int confirmerId)
        {
            await _holidayValidationService.ValidateHolidayConfirmationReadiness(holidayId, confirmerId);
        }
        [Theory]
        [InlineData(10, 1)]
        [InlineData(11, 1)]
        [InlineData(12, 1)]
        [InlineData(13, 1)]
        [InlineData(14, 1)]
        public async void When_ValidatingHolidayConfirmationReadiness_Expect_ValidationFail(int holidayId, int confirmerId)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => _holidayValidationService.ValidateHolidayConfirmationReadiness(holidayId, confirmerId));
        }
        [Theory]
        [InlineData(15)]
        [InlineData(16)]
        public async void When_ValidatingNewHolidayConfirmationReadiness_Expect_ValidationSuccess(int holidayId)
        {

            var holiday = await _holidaysRepository.GetById(holidayId);
            var holidayDto = _mapper.Map<NewHolidayDto>(holiday);

            await _holidayValidationService.ValidateNewHolidayConfirmationReadiness(holidayDto);
        }
        [Theory]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        [InlineData(14)]
        public async void When_ValidatingNewHolidayConfirmationReadiness_Expect_ValidationFail(int holidayId)
        {

            var holiday = await _holidaysRepository.GetById(holidayId);
            var holidayDto = _mapper.Map<NewHolidayDto>(holiday);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _holidayValidationService.ValidateNewHolidayConfirmationReadiness(holidayDto));
        }
        [Theory]
        [InlineData(-1)]
        [InlineData(-2)]
        public async void When_ValidatingNewHolidayConfirmationReadiness_Expect_NullException(int holidayId)
        {

            var holiday = await _holidaysRepository.GetById(holidayId);
            var holidayDto = _mapper.Map<NewHolidayDto>(holiday);

            await Assert.ThrowsAsync<ArgumentNullException>(() => _holidayValidationService.ValidateNewHolidayConfirmationReadiness(holidayDto));
        }
    }

}
