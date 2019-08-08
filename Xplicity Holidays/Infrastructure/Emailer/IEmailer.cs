namespace Xplicity_Holidays.Infrastructure.Emailer
{
    public interface IEmailer
    {
        void SendMail(string mailTo, string subject, string body, string displayName);
    }
}
