using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.EntityFrameworkCore;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly HolidayDbContext _context;

        public FileRepository(HolidayDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<File>> GetAll()
        {
            var files = await _context.Files.ToArrayAsync();
            return files;
        }

        public async Task<File> GetById(int id)
        {
            var file = await _context.Files.FindAsync(id);
            return file;
        }

        public async Task<int> Create(File newFile)
        {
            await _context.Files.AddAsync(newFile);
            await _context.SaveChangesAsync();
            return newFile.Id;
        }

        public async Task<bool> Update(File fileToUpdate)
        {
            _context.Files.Attach(fileToUpdate);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<bool> Delete(File fileToDelete)
        {
            _context.Files.Remove(fileToDelete);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<File> FindByType(string fileType)
        {
            var file = await _context.Files.Where(f => f.Type == fileType).OrderBy(f => f.CreatedAt).LastAsync();
            return file;
        }
    }
}
