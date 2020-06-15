using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public class ChoicesRepository : IChoicesRepository
    {
        protected readonly HolidayDbContext _context;

        public ChoicesRepository(HolidayDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Choice>> GetAll()
        {
            var surveys = await _context.Choices.ToArrayAsync();

            return surveys;
        }

        public async Task<Choice> GetById(int id)
        {
            var survey = await _context.Choices.FindAsync(id);
            return survey;
        }

        public async Task<int> Create(Choice entity)
        {
            _context.Choices.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> Update(Choice entity)
        {
            _context.Choices.Attach(entity);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<bool> Delete(Choice entity)
        {
            _context.Choices.Remove(entity);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }
    }
}
