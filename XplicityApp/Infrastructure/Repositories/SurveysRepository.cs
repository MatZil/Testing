using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public class SurveysRepository : ISurveysRepository
    {
        protected readonly HolidayDbContext _context;

        public SurveysRepository(HolidayDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Survey>> GetAll()
        {
            var surveys = await _context.Surveys.ToArrayAsync();

            return surveys;
        }

        public async Task<Survey> GetById(int id)
        {
            var survey = await _context.Surveys.FindAsync(id);
            return survey;
        }
        public async Task<Survey> GetByGuid(string guid)
        {
            var survey = await _context.Surveys
                .Where(survey => survey.Guid.Equals(guid))
                .Select(survey => survey)
                .Include(q => q.Questions)
                .ThenInclude(x => x.Choices)
                .FirstAsync();

            return survey;
        }

        public async Task<int> Create(Survey entity)
        {
            _context.Surveys.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> Update(Survey entity)
        {
            _context.Surveys.Attach(entity);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<bool> Delete(Survey entity)
        {
            _context.Surveys.Remove(entity);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }
    }
}
