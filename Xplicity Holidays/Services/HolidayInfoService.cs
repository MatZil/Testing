using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class HolidayInfoService : IHolidayInfoService
    {
        private readonly IEmployeeRepository _repository;

        public HolidayInfoService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public List<(string, double)> GetAllEmployeesHolidaysLeft()
        {
            List<Employee> employees = _repository.GetAll().Result.ToList();

            if (employees.Count == 0)
                return null;

            List<(string, double)> holidayDays = new List<(string, double)>();

            foreach (var employee in employees)
            {
                double holidaysLeft = GetNumberOfHolidaysLeft(employee.Id);
                holidayDays.Add((employee.Name + " " + employee.Surname, holidaysLeft));
            }

            return holidayDays;
        }

        public double GetNumberOfHolidaysLeft(int id)
        {
            var employee = _repository.GetById(id).Result;

            if (employee == null)
                return 0;

            double holidaysPerYear = employee.DaysOfVacation;
            double daysInYear = 365;
            DateTime startCheckFrom;

            //if (employee.LastCheckDate == null)
                startCheckFrom = employee.WorksFromDate;
            //else
            //    startCheckFrom = employee.LastCheckDate;

            int workDays = CalculateWorkDays(startCheckFrom, DateTime.Now);
            List<Holiday> employeesHolidays = _repository.GetHolidays(employee.Id);
            int holidayCount = employeesHolidays.Sum(holiday => (holiday.ToExclusive - holiday.FromInclusive).Days);
            double totalWorkDays = workDays - holidayCount;

            double holidaysLeft = (holidaysPerYear / daysInYear) * totalWorkDays;
            //employee.LastCheckDate = DateTime.Now;
            //_repository.Update(employee);
            return holidaysLeft;

        }

        private int CalculateWorkDays(DateTime dateFrom, DateTime dateTo)
        {
            int workDays = 0;

            while(dateFrom < dateTo)
            {
                if (dateFrom.DayOfWeek == DayOfWeek.Saturday || dateFrom.DayOfWeek == DayOfWeek.Sunday)
                {
                    dateFrom = dateFrom.AddDays(1);
                    continue;
                }

                workDays++;
                dateFrom = dateFrom.AddDays(1);
            }
            workDays--; //Subtract today

            return workDays;
        }

    }
}
