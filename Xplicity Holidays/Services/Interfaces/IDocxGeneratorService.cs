﻿using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IDocxGeneratorService
    {
        Task<int> GenerateHolidayDocx(int holidayId, FileTypeEnum holidayDocumentType);
    }
}
