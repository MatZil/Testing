using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Xplicity_Holidays.Infrastructure.Repositories
{
    public interface IFileRepository : IRepository<File>
    {
        Task<File> FindByType(FileTypeEnum fileType);
    }
}
