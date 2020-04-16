using Microsoft.AspNetCore.Mvc.RazorPages;

namespace XplicityApp
{
    public class HolidayConfirmationResultModel : PageModel
    {
        public bool Confirmed { get; private set; }
        public string ImagePath;
        public string ConfirmationText;

        public void OnGet(bool confirmed)
        {
            Confirmed = confirmed;

            if (confirmed)
            {
                ImagePath = Url.Content("~/Resources/Images/requestConfirmed.png");
                ConfirmationText = "You confirmed the request.";
            }
            else
            {
                ImagePath = Url.Content("~/Resources/Images/requestRejected.png");
                ConfirmationText = "You rejected the request.";
            }
        }
    }
}