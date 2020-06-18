using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public class QuestionsRepository : IQuestionsRepository
    {
        protected readonly HolidayDbContext _context;

        public QuestionsRepository(HolidayDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Question>> GetAll()
        {
            var questions = await _context.Questions.ToArrayAsync();

            return questions;
        }

        public async Task<Question> GetById(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            return question;
        }

        public async Task<int> Create(Question entity)
        {
            _context.Questions.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> Update(Question entity)
        {
            _context.Questions.Attach(entity);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<bool> Delete(Question entity)
        {
            _context.Questions.Remove(entity);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }

        public Task<ICollection<Question>> GetAllBySurveyId(int surveyId)
        {
            throw new System.NotImplementedException();
        }
    }
}
