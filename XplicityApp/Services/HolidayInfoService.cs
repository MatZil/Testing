using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
{
    public class HolidayInfoService : IHolidayInfoService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRepository<Client> _clientRepository;

        public HolidayInfoService(IEmployeeRepository repository, IRepository<Client> clientRepository)
        {
            _employeeRepository = repository;
            _clientRepository = clientRepository;
        }

        public async Task<List<(Holiday, Client)>> GetClientsAndHolidays(ICollection<Holiday> holidays)
        {
            List<(Holiday, Client)> clientsWithHolidays = new List<(Holiday, Client)>();
            foreach (var holiday in holidays)
            {
                var employee = await _employeeRepository.GetById(holiday.EmployeeId);
                if (employee.ClientId != null)
                {
                    var client = await _clientRepository.GetById((int)employee.ClientId);
                    clientsWithHolidays.Add((holiday, client));
                }
                else
                {
                    clientsWithHolidays.Add((holiday, null));
                }
            }

            return clientsWithHolidays;
        }
    }
}
