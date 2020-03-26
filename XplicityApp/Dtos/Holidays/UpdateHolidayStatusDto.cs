using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XplicityApp.Dtos.Holidays
{
    public class UpdateHolidayStatusDto
    {
        public bool Confirm { get; set; }
        public int HolidayId { get; set; }
        public int ConfirmerId { get; set; }
        public bool IsConfirmerAdmin { get; set; }
        public string RejectionReason { get; set; }
    }
}
