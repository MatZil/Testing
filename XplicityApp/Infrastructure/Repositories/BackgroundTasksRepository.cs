using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public class BackgroundTasksRepository : IBackgroundTasksRepository
    {
        protected readonly HolidayDbContext _context;

        public BackgroundTasksRepository(HolidayDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<BackgroundTask>> GetAll()
        {
            var surveys = await _context.BackgroundTasks.ToArrayAsync();

            return surveys;
        }

        public async Task<BackgroundTask> GetById(int id)
        {
            var survey = await _context.BackgroundTasks.FindAsync(id);
            return survey;
        }

        public async Task<int> Create(BackgroundTask entity)
        {
            _context.BackgroundTasks.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> Update(BackgroundTask entity)
        {
            _context.BackgroundTasks.Attach(entity);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<bool> Delete(BackgroundTask entity)
        {
            _context.BackgroundTasks.Remove(entity);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }
    }
}
