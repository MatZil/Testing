using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public class HolidayGuidsRepository : IHolidayGuidsRepository
    {
        protected readonly HolidayDbContext Context;

        public HolidayGuidsRepository(HolidayDbContext context)
        {
            Context = context;
        }

        public async Task<ICollection<HolidayGuid>> GetAll()
        {
            var holidayGuids = await Context.HolidayGuids.ToArrayAsync();
            return holidayGuids;
        }

        public async Task<HolidayGuid> GetById(int id)
        {
            var holidayGuid = await Context.HolidayGuids.FindAsync(id);
            return holidayGuid;
        }

        public async Task<int> Create(HolidayGuid newHolidayGuid)
        {
            Context.HolidayGuids.Add(newHolidayGuid);
            await Context.SaveChangesAsync();

            return newHolidayGuid.Id;
        }

        public async Task<bool> Update(HolidayGuid holidayGuid)
        {
            Context.HolidayGuids.Attach(holidayGuid);
            var changes = await Context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<bool> Delete(HolidayGuid holidayGuid)
        {
            Context.HolidayGuids.Remove(holidayGuid);
            var changes = await Context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<string> GetGuid(int holidayId, int confirmerId, bool isAdmin)
        {
            var holidayGuid = await Context.HolidayGuids
                       .Where(guid => guid.HolidayId == holidayId && guid.ConfirmerId == confirmerId && guid.IsAdmin == isAdmin)
                       .Select(guid => guid.Guid).FirstAsync();

            return holidayGuid;
        }



        public async Task<HolidayGuid> GetHolidayGuid(string guid)
        {
            try
            {
                var holidayGuid = await Context.HolidayGuids
                              .Where(holidayGuid => holidayGuid.Guid.Equals(guid))
                              .Select(holidayGuid => holidayGuid).FirstAsync();

                return holidayGuid;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteGuids(int holidayId, bool isAdmin)
        {
            if (isAdmin)
            {
                var holidayGuids = Context.HolidayGuids
                                   .Where(holidayGuid => holidayGuid.HolidayId == holidayId && holidayGuid.IsAdmin == true)
                                   .Select(holidayGuid => holidayGuid).ToList();

                foreach (var guid in holidayGuids)
                {
                    await Delete(guid);
                    return true;
                }
            }
            else
            {
                var holidayGuid = await Context.HolidayGuids
                                  .Where(holidayGuid => holidayGuid.HolidayId == holidayId && holidayGuid.IsAdmin == false)
                                  .Select(holidayGuid => holidayGuid).FirstAsync();

                if (holidayGuid != null)
                {
                    await Delete(holidayGuid);
                    return true;
                }
            }

            return false;
        }
    }
}
