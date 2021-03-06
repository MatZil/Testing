﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using XplicityApp.Dtos.Employees;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace XplicityApp.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ITimeService _timeService;
        private readonly IUserService _userService;
        private readonly IOvertimeUtility _overtimeUtility;
        private readonly INotificationSettingsService _notificationSettingsService;
        private readonly IConfiguration _configuration;

        public EmployeesService(
            IEmployeeRepository repository,
            IMapper mapper,
            IOvertimeUtility overtimeUtility,
            ITimeService timeService,
            IUserService userService,
            INotificationSettingsService notificationSettingsService,
            IConfiguration configuration
            )
        {
            _repository = repository;
            _mapper = mapper;
            _userService = userService;
            _timeService = timeService;
            _overtimeUtility = overtimeUtility;
            _notificationSettingsService = notificationSettingsService;
            _configuration = configuration;
        }

        public async Task<GetEmployeeDto> GetById(int id)
        {
            var employee = await _repository.GetById(id);
            var employeeDto = _mapper.Map<GetEmployeeDto>(employee);

            return employeeDto;
        }

        public async Task<ICollection<GetEmployeeDto>> GetAll()
        {
            var employees = await _repository.GetAll();
            var employeesDto = _mapper.Map<GetEmployeeDto[]>(employees);

            foreach (var employee in employeesDto)
            {
                employee.Role = await _userService.GetUserRole(employee.Id);
            }

            return employeesDto;
        }

        public async Task<ICollection<GetEmployeeDto>> GetByEmployeeStatus(EmployeeStatusEnum employeeStatus)
        {
            var employees = await _repository.GetByEmployeeStatus(employeeStatus);
            var employeesDto = _mapper.Map<GetEmployeeDto[]>(employees);

            foreach (var employee in employeesDto)
            {
                employee.Role = await _userService.GetUserRole(employee.Id);
            }

            return employeesDto;
        }

        public async Task<NewEmployeeDto> Create(NewEmployeeDto newEmployeeDto)
        {
            if (newEmployeeDto == null)
            {
                throw new ArgumentNullException(nameof(newEmployeeDto));
            }

            var password = newEmployeeDto.Password;

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Password is required");
            }

            if (_repository.FindByEmail(newEmployeeDto.Email).Result != null)
            {
                throw new Exception("Email \"" + newEmployeeDto.Email + "\" is already taken");
            }

            var newEmployee = _mapper.Map<Employee>(newEmployeeDto);

            if (!newEmployeeDto.IsManualHolidaysInput)
            {
                var currentTime = _timeService.GetCurrentTime();
                var workedTime = _timeService.GetWorkDays(newEmployee.WorksFromDate, currentTime);
                var workDaysPerYear = _timeService.GetCurrentYearWorkDays();
                newEmployee.FreeWorkDays = Math.Round(workedTime * ((double)newEmployee.DaysOfVacation / workDaysPerYear), 2);
            }
            else
            {
                if (newEmployeeDto.FreeWorkDays is null)
                {
                    throw new ArgumentNullException();
                }

                newEmployee.FreeWorkDays = (double)newEmployeeDto.FreeWorkDays;
            }


            newEmployee.CurrentAvailableLeaves = newEmployee.ParentalLeaveLimit;
            newEmployee.NextMonthAvailableLeaves = newEmployee.ParentalLeaveLimit;

            await _repository.Create(newEmployee);
            await _userService.Create(newEmployee, newEmployeeDto);
            await _notificationSettingsService.Create(newEmployee.Id);

            var employeeDto = _mapper.Map<NewEmployeeDto>(newEmployee);

            return employeeDto;
        }
        public async Task<bool> Delete(int id)
        {
            var item = await _repository.GetById(id);

            if (item == null)
                return false;

            var deleted = await _repository.Delete(item);

            return deleted;
        }

        public async Task Update(int id, UpdateEmployeeDto updateData)
        {
            if (updateData == null)
                throw new ArgumentNullException(nameof(updateData));

            var employeeToUpdate = await _repository.GetById(id);

            if (employeeToUpdate == null)
            {
                throw new InvalidOperationException();
            }

            if (updateData.FreeWorkDays != employeeToUpdate.FreeWorkDays)
            {
                employeeToUpdate.FreeWorkDays = updateData.FreeWorkDays;
            }

            if (updateData.Email != employeeToUpdate.Email)
            {
                // email has changed so check if the new email is already taken
                if (_repository.FindByEmail(updateData.Email).Result != null)
                    throw new Exception("Email " + updateData.Email + " is already taken");

                await _userService.ChangeEmail(id, updateData.Email);
            }

            var parentalLeaveDifference = updateData.ParentalLeaveLimit - employeeToUpdate.ParentalLeaveLimit;
            employeeToUpdate.CurrentAvailableLeaves = Math.Max(employeeToUpdate.CurrentAvailableLeaves + parentalLeaveDifference, 0);
            employeeToUpdate.NextMonthAvailableLeaves = Math.Max(employeeToUpdate.NextMonthAvailableLeaves + parentalLeaveDifference, 0);

            _mapper.Map(updateData, employeeToUpdate);

            await _repository.Update(employeeToUpdate);
            await _userService.Update(id, updateData);
        }

        public Employee AddOvertimeDays(Employee employee)
        {
            return _overtimeUtility.AddOvertimeDaysToEmployee(employee);
        }

        public bool HasActiveUnpaidHoliday(int employeeId)
        {
            var currentTime = _timeService.GetCurrentTime();
            var employeeHolidays = _repository.GetConfirmedHolidays(employeeId);

            foreach (var holiday in employeeHolidays)
            {
                if (holiday.FromInclusive <= currentTime && holiday.ToInclusive >= currentTime && holiday.Type == HolidayType.Unpaid)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _repository.EmailExists(email);
        }


        public async Task<List<GetEmployeeBirthdayDto>> GetBirthdaysByMonth(DateTime selectedDate, int currentUserId)
        {
            var dateFrom = _timeService.GetCalendarDateFrom(_configuration, selectedDate);
            var dateTo = _timeService.GetCalendarDateTo(_configuration, selectedDate);

            var allEmployees = await _repository.GetAll();
            var selectedMonthBirthdays = new List<GetEmployeeBirthdayDto>();

            foreach (var employee in allEmployees)
            {
                var birthday = await GetBirthdayInformation(employee, selectedDate);
                bool datesOverlap = dateFrom < birthday.BirthdayDate && birthday.BirthdayDate <= dateTo;
                if (datesOverlap)
                {
                    selectedMonthBirthdays.Add(birthday);
                }
            }

            var selectedMonthBirthdaysByRole = await GetByRole(currentUserId, selectedMonthBirthdays);

            return selectedMonthBirthdaysByRole;
        }

        public async Task<List<GetEmployeeBirthdayDto>> GetByRole(int currentUserId, List<GetEmployeeBirthdayDto> allBirthdays)
        {
            var employeeRole = await _userService.GetUserRole(currentUserId);

            if (employeeRole != "Admin")
            {
                var publicBirthdays = new List<GetEmployeeBirthdayDto>();
                foreach (var birthday in allBirthdays)
                {
                    if (birthday.IsPublic)
                    {
                        publicBirthdays.Add(birthday);
                    }
                }
                return publicBirthdays;
            }

            return allBirthdays;
        }

        private async Task<GetEmployeeBirthdayDto> GetBirthdayInformation(Employee employee, DateTime selectedDate)
        {
            var notificationSettings = await _notificationSettingsService.GetByEmployeeId(employee.Id);

            var birthdayDate = employee.BirthdayDate;
            var adjustedBirthdayDate = new DateTime(selectedDate.Year, birthdayDate.Month, birthdayDate.Day);

            var birthday = new GetEmployeeBirthdayDto()
            {
                FullName = $"{employee.Name} {employee.Surname}",
                BirthdayDate = adjustedBirthdayDate,
                IsPublic = notificationSettings.BroadcastOwnBirthday
            };

            return birthday;
        }
    }
}