using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IBackgroundService
    {
        void CheckUpcomingHolidays(List<Employee> employees, List<Holiday> holidays);

        void CheckForLastMonthDay(Employee admin, List<Holiday> holidays);

        Task RunBackgroundServices();
    }
}
