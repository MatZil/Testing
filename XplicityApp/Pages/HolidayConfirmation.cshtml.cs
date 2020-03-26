using AutoMapper;
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

        public async Task OnGetAsync(int holidayId, int confirmerId)
        {
                //url for this page is {{RootUrl}}/HolidayConfirmation?holidayid=X&confirmerid=Y
                ConfirmerId = confirmerId;
                HolidayId = holidayId;

                var holiday = await _context.Holidays.FindAsync(holidayId);
                HolidayFrom = holiday.FromInclusive;
                HolidayTo = holiday.ToInclusive;

                var employee = await _context.Employees.FindAsync(holiday.EmployeeId);
                RequesterName = $"{employee.Name} {employee.Surname}";

                if (employee.ClientId == null || holiday.Status == HolidayStatus.ClientConfirmed)
                {
                    IsConfirmerAdmin = true;
                }
                else
                {
                    IsConfirmerAdmin = false;
                }
        }

        public async Task<IActionResult> OnPostAsync(bool confirm, int holidayId, int confirmerId, bool isConfirmerAdmin)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Console.WriteLine($"{RejectionReason} {confirm} {holidayId} {confirmerId} {isConfirmerAdmin}");
            var updatedHolidayStatusDto = new UpdateHolidayStatusDto()
            {
                Confirm = confirm,
                HolidayId = holidayId,
                ConfirmerId = confirmerId,
                IsConfirmerAdmin = isConfirmerAdmin,
                RejectionReason = RejectionReason
            };
            //consider creating a holiday confirmation object to call the service with
            await _holidayConfirmationService.UpdateHolidayConfirmationStatus(updatedHolidayStatusDto);

            return RedirectToPage("/HolidayConfirmationResult");
        }
    }
}