namespace XplicityApp.Infrastructure.Emailer
{
    public interface IEmailer
    {
        void SendMail(string mailTo, string subject, string body);
    }
}
