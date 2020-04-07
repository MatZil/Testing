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