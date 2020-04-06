using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace XplicityApp
{
    
    public class HolidayConfirmationAbandonedModel : PageModel
    {
        public string UserWhoAbandoned { get; private set; }
        public void OnGet(string userWhoAbandoned)
        {
            UserWhoAbandoned = userWhoAbandoned;
        }
    }
}