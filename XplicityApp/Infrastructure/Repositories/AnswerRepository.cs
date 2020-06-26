using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly HolidayDbContext _context;

        public AnswerRepository(HolidayDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Answer entity)
        {
            await _context.Answers.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public Task<bool> Delete(Answer entity)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Answer>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Answer> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Answer entity)
        {
            throw new NotImplementedException();
        }
    }
}
