using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp
{
    public class HolidayConfirmationModel : PageModel
    {
        private readonly HolidayDbContext Context;

        public int ConfimerId { get; set; }
        public int HolidayId { get; set; }

        public Employee Employee { get; set; }

        public Holiday Holiday { get; set; }

        public bool isConfimerAdmin { get; set; }

        public HolidayConfirmationModel(HolidayDbContext context)
        {
            Context = context;
        }
        public async Task OnGetAsync([FromRoute]int holidayId, [FromRoute]int confirmerId)
        {
            ConfimerId = confirmerId;
            HolidayId = holidayId;

            Holiday = await Context.Holidays.FindAsync(holidayId);
            Employee = await Context.Employees.FindAsync(Holiday.EmployeeId);
            if (Employee.ClientId == null || Holiday.Status == HolidayStatus.ClientConfirmed)
            {
                isConfimerAdmin = true;
            }
            else
            {
                isConfimerAdmin = false;
            }
        }
    }
}