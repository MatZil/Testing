using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.EntityFrameworkCore;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Xplicity_Holidays.Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly HolidayDbContext _context;

        public FileRepository(HolidayDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<FileRecord>> GetAll()
        {
            var files = await _context.FileRecords.ToArrayAsync();
            return files;
        }

        public async Task<FileRecord> GetById(int id)
        {
            var fileRecord = await _context.FileRecords.FindAsync(id);
            return fileRecord;
        }

        public async Task<int> Create(FileRecord newFileRecord)
        {
            await _context.FileRecords.AddAsync(newFileRecord);
            await _context.SaveChangesAsync();
            return newFileRecord.Id;
        }

        public async Task<bool> Update(FileRecord fileRecordToUpdate)
        {
            _context.FileRecords.Attach(fileRecordToUpdate);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<bool> Delete(FileRecord fileRecordToDelete)
        {
            _context.FileRecords.Remove(fileRecordToDelete);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<FileRecord> FindByType(FileTypeEnum fileType)
        {
            var file = await _context.FileRecords.Where(f => f.Type == fileType).OrderBy(f => f.CreatedAt).LastAsync();
            return file;
        }
    }
}
