using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using XplicityApp.Dtos.Employees;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ITimeService _timeService;
        private readonly IUserService _userService;
        private readonly IOvertimeUtility _overtimeUtility;

        public EmployeesService(IEmployeeRepository repository, IMapper mapper, IOvertimeUtility overtimeUtility,
                                ITimeService timeService, IUserService userService)
        {
            _repository = repository;
            _mapper = mapper;
            _userService = userService;
            _timeService = timeService;
            _overtimeUtility = overtimeUtility;
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
            }

            var parentalLeaveDifference = updateData.ParentalLeaveLimit - employeeToUpdate.ParentalLeaveLimit;
            employeeToUpdate.CurrentAvailableLeaves = Math.Max(employeeToUpdate.CurrentAvailableLeaves + parentalLeaveDifference, 0);
            employeeToUpdate.NextMonthAvailableLeaves = Math.Max(employeeToUpdate.NextMonthAvailableLeaves + parentalLeaveDifference, 0);

            _mapper.Map(updateData, employeeToUpdate);

            await _repository.Update(employeeToUpdate);
            await _userService.Update(id, updateData);
        }

        public Employee AddOvertimeDetails(Employee employee)
        {
            return _overtimeUtility.AddOvertimeDetailsToEmployee(employee);
        }

        public bool HasActiveUnpaidHoliday(int employeeId)
        {
            var currentTime = _timeService.GetCurrentTime();
            var employeeHolidays = _repository.GetConfirmedHolidays(employeeId);

            foreach (var holiday in employeeHolidays)
            {
                if (holiday.FromInclusive <= currentTime && holiday.ToInclusive >= currentTime && !holiday.Paid)
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
    }
}
