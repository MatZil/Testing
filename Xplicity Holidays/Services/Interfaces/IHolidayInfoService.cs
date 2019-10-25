﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IHolidayInfoService
    {
        Task<List<(Holiday, Client)>> GetClientsAndHolidays(ICollection<Holiday> holidays);
    }
}
