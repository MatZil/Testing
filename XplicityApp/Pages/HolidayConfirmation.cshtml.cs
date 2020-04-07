using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using XplicityApp.Dtos.Holidays;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Pages
{
    [UsedImplicitly]
    public class HolidayConfirmationModel : PageModel
    {
        private readonly HolidayDbContext _context;
        private readonly IHolidayConfirmService _holidayConfirmationService;

        public int ConfirmerId { get; private set; }
        public int HolidayId { get; private set; }
        public string RequesterName { get; private set; }
        public DateTime HolidayFrom { get; private set; }
        public DateTime HolidayTo { get; private set; }

        [BindProperty]
        public string RejectionReason { get; set; }

        public bool IsConfirmerAdmin { get; private set; }

        public HolidayConfirmationModel(HolidayDbContext context, IHolidayConfirmService holidayConfirmationService)
        {
            _context = context;
            _holidayConfirmationService = holidayConfirmationService;
        }

        public async Task<IActionResult> OnGetAsync(int holidayId, int confirmerId)
        {

            ConfirmerId = confirmerId;
            HolidayId = holidayId;

            var holiday = await _context.Holidays.FindAsync(holidayId);
            HolidayFrom = holiday.FromInclusive;
            HolidayTo = holiday.ToInclusive;

            var employee = await _context.Employees.FindAsync(holiday.EmployeeId);
            RequesterName = $"{employee.Name} {employee.Surname}";
            if (holiday.Status == HolidayStatus.Abandoned)
            {
                return RedirectToPage("HolidayConfirmationAbandoned", new { userWhoAbandoned = RequesterName });
            }
            IsConfirmerAdmin = employee.ClientId == null || holiday.Status == HolidayStatus.ClientConfirmed;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool confirm, int holidayId, int confirmerId, bool isConfirmerAdmin)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var updatedHolidayStatusDto = new UpdateHolidayStatusDto()
            {
                Confirm = confirm,
                HolidayId = holidayId,
                ConfirmerId = confirmerId,
                IsConfirmerAdmin = isConfirmerAdmin,
                RejectionReason = RejectionReason
            };

            await _holidayConfirmationService.UpdateHolidayConfirmationStatus(updatedHolidayStatusDto);

            return RedirectToPage("/HolidayConfirmationResult");
        }
    }
}