using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Xplicity_Holidays.Dtos.Employees;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class EmployeesService: IEmployeesService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authenticationService;

        public EmployeesService(IEmployeeRepository repository, IAuthService authenticationService, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public Employee Authenticate(string email, string password)
        {
            var employee = _authenticationService.Authenticate(_repository, email, password);
            _repository.Update(employee);

            return employee;
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
            return employeesDto;
        }

        public async Task<NewEmployeeDto> Create(NewEmployeeDto newEmployeeDto)
        {
            if (newEmployeeDto == null) throw new ArgumentNullException(nameof(newEmployeeDto));
            string password = newEmployeeDto.Password;

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required");

            if (_repository.FindByEmail(newEmployeeDto.Email).Result != null)
                throw new Exception("Email \"" + newEmployeeDto.Email + "\" is already taken");

            var newEmployee = _mapper.Map<Employee>(newEmployeeDto);

            byte[] passwordHash, passwordSalt;
            _authenticationService.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            newEmployee.PasswordHash = passwordHash;
            newEmployee.PasswordSalt = passwordSalt;

            await _repository.Create(newEmployee);

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

        public async Task Update(int id, NewEmployeeDto updateData)
        {
            if (updateData == null)
                throw new ArgumentNullException(nameof(updateData));

            var itemToUpdate = await _repository.GetById(id);

            if (itemToUpdate == null)
                throw new InvalidOperationException();

            if (updateData.Email != itemToUpdate.Email)
            {
                // email has changed so check if the new email is already taken
                if (_repository.FindByEmail(updateData.Email).Result != null)
                    throw new Exception("Email " + updateData.Email + " is already taken");
            }

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(updateData.Password))
            {
                byte[] passwordHash, passwordSalt;
                _authenticationService.CreatePasswordHash(updateData.Password, out passwordHash, out passwordSalt);

                itemToUpdate.PasswordHash = passwordHash;
                itemToUpdate.PasswordSalt = passwordSalt;
            }

            _mapper.Map(updateData, itemToUpdate);
            await _repository.Update(itemToUpdate);
        }
    }
}
