using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public class EmailTemplatesRepository: IEmailTemplatesRepository
    {
        protected readonly HolidayDbContext _context;

        public EmailTemplatesRepository(HolidayDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<EmailTemplate>> GetAll()
        {
            var emailTemplates = await _context.EmailTemplates.ToArrayAsync();

            return emailTemplates;
        }

        public async Task<EmailTemplate> GetById(int id)
        {
            var emailTemplate = await _context.EmailTemplates.FindAsync(id);

            return emailTemplate;
        }

        public async Task<int> Create(EmailTemplate newEmailTemplate)
        {
            _context.EmailTemplates.Add(newEmailTemplate);
            await _context.SaveChangesAsync();

            return newEmailTemplate.Id;
        }

        public async Task<bool> Update(EmailTemplate emailTemplate)
        {
            _context.EmailTemplates.Attach(emailTemplate);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<bool> Delete(EmailTemplate emailTemplate)
        {
            _context.EmailTemplates.Remove(emailTemplate);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<EmailTemplate> GetByPurpose(string purpose)
        {
            var emailTemplate = await _context.EmailTemplates.FirstOrDefaultAsync(template => template.Purpose == purpose);

            return emailTemplate;
        }
    }
}
