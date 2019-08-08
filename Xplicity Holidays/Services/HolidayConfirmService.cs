using System;
using AutoMapper;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Emailer;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class HolidayConfirmService: IHolidayConfirmService
    {
        private readonly IEmailer _emailer;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _repositoryEmployees;
        private readonly IRepository<Client> _repositoryClients;
        public HolidayConfirmService(IEmailer emailer, IMapper mapper, 
            IEmployeeRepository repositoryEmployees, IRepository<Client> repositoryClients)
        {
            _emailer = emailer;
            _mapper = mapper;
            _repositoryEmployees = repositoryEmployees;
            _repositoryClients = repositoryClients;
        }
        public async void RequestClientApproval(NewHolidayDto holidayDto)
        {
            var holiday = _mapper.Map<Holiday>(holidayDto);
            var employee = await _repositoryEmployees.GetById(holiday.EmployeeId);

            if (employee.ClientId == null)
                return;

            var client = await _repositoryClients.GetById(employee.ClientId.GetValueOrDefault());
            _emailer.SendMail(client.OwnerEmail, "A holiday request from your employee",
                $"Hello, {client.OwnerName}\n\nOne of your employees, {employee.Name} {employee.Surname}, is intending to go " +
                $"for holidays from {holiday.From.ToShortDateString()} to {holiday.To.ToShortDateString()}.\n\nPlease click this link to confirm:");
        }
      // void RequestAdminApproval(int holidayId, int adminId);
    }
}
