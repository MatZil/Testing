﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Dtos.Holidays;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Services.Interfaces
{
    public interface IHolidaysService
    {
        Task<GetHolidayDto> GetById(int id);
        Task<ICollection<GetHolidayDto>> GetAll();
        Task<int> Create(NewHolidayDto newClient);
        Task<bool> Update(int id, UpdateHolidayDto updatedHoliday);
        Task<bool> Delete(int id);
        Task<bool> Decline(int holidayId, int confirmerId);
        Task<ICollection<GetHolidayDto>> GetByEmployeeStatus(EmployeeStatusEnum employeeStatus);
    }
}
