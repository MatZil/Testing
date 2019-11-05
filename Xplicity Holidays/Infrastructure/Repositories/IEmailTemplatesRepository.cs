using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Infrastructure.Repositories
{
    public interface IEmailTemplatesRepository: IRepository<EmailTemplate>
    {
        Task<EmailTemplate> GetByPurpose(string purpose);
    }
}
