using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace XplicityApp.Infrastructure.Emailer
{
    public class Emailer: IEmailer
    {
        private readonly IConfiguration _configuration;
        public Emailer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMail(string mailTo, string subject, string body)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_configuration["EmailConfig:FromEmail"], 
                                    _configuration["EmailConfig:DisplayName"]),
                Subject = subject,
                Body = body
            };

            message.To.Add(mailTo);

            using (var client = new SmtpClient())
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
