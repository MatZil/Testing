using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public class TagsRepository : ITagsRepository
    {
        protected readonly HolidayDbContext _context;

        public TagsRepository(HolidayDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Tag entity)
        {
            _context.Tags.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public Task<bool> Delete(Tag entity)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ICollection<Tag>> FindByTitle(string tagTitle)
        {
            var tags = await _context.Tags.Where(tag => tag.Title.Contains(tagTitle)).ToArrayAsync();

            return tags;
        }

        public async Task<ICollection<Tag>> GetAll()
        {
            var tags = await _context.Tags.ToArrayAsync();

            return tags;
        }

        public async Task<Tag> GetById(int id)
        {
            var tag = await _context.Tags.FindAsync(id);

            return tag;
        }

        public async Task<bool> TagExists(string tagTitle)
        {
            var tags = await _context.Tags.Where(tag => tag.Title.Equals(tagTitle)).ToArrayAsync();

            return tags.Any();
        }

        public Task<bool> Update(Tag entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
