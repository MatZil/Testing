using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;


namespace Xplicity_Holidays.Infrastructure.Emailer
{
    public class Emailer: IEmailer
    {
        private IConfiguration _configuration;
        public Emailer(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendMail(string mailTo, string subject, string body, string displayName)
        {
            MailMessage message = new MailMessage()
            {
                From = new MailAddress(_configuration["EmailConfig:FromEmail"], displayName),
                Subject = subject,
                Body = body
            };
            message.To.Add(mailTo);
            using (SmtpClient client = new SmtpClient())
            {
                client.Port = _configuration.GetValue<int>("EmailConfig:Port");
                client.Host = _configuration["EmailConfig:Host"];
                client.EnableSsl = _configuration.GetValue<bool>("EmailConfig:EnableSsl");
                client.Credentials = new NetworkCredential(_configuration["EmailConfig:Username"], 
                                                            _configuration["EmailConfig:Password"]);
                client.Send(message);
            }

        }
    }
}
