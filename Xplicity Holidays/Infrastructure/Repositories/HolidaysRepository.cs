using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Xplicity_Holidays.Infrastructure.Repositories
{
    public class HolidaysRepository : IHolidaysRepository
    {
        protected readonly HolidayDbContext Context;

        public HolidaysRepository(HolidayDbContext context)
        {
            Context = context;
        }
        public async Task<ICollection<Holiday>> GetByEmployeeStatus(EmployeeStatusEnum employeeStatus)
        {
            var holidays = await Context.Holidays.Include(x => x.Employee).Where(x => x.Employee.Status == employeeStatus)
                .ToArrayAsync();
            return holidays;
        }

        public async Task<ICollection<Holiday>> GetAll()
        {
            var holidays = await Context.Holidays.ToArrayAsync();
            return holidays;
        }

        public async Task<Holiday> GetById(int id)
        {
            var holiday = await Context.Holidays.FindAsync(id);
            return holiday;
        }

        public async Task<int> Create(Holiday newHoliday)
        {
            Context.Holidays.Add(newHoliday);
            await Context.SaveChangesAsync();

            return newHoliday.Id;
        }

        public async Task<bool> Update(Holiday holiday)
        {
            Context.Holidays.Attach(holiday);
            var changes = await Context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<bool> Delete(Holiday holiday)
        {
            Context.Holidays.Remove(holiday);
            var changes = await Context.SaveChangesAsync();

            return changes > 0;
        }
    }
}
