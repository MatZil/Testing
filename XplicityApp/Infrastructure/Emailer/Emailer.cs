using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using System;

namespace XplicityApp.Infrastructure.Emailer
{
    public class Emailer: IEmailer
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public Emailer(IConfiguration configuration, ILogger<Emailer> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void SendMail(string mailTo, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(_configuration["EmailConfig:DisplayName"], 
                    _configuration["EmailConfig:FromEmail"]));

                message.To.Add(new MailboxAddress("Recipient", mailTo));

                message.Subject = subject;

                message.Body = new TextPart(TextFormat.Text)
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(_configuration["EmailConfig:Host"],
                        _configuration.GetValue<int>("EmailConfig:Port"),
                        SecureSocketOptions.SslOnConnect);

                    client.Authenticate(_configuration["EmailConfig:FromEmail"], 
                        _configuration["EmailConfig:AppPassword"]);

                    client.Send(message);

                    client.Disconnect(true);

                    _logger.LogInformation($"Email messsage succesfully sent " +
                        $"from {_configuration["EmailConfig:FromEmail"]} to {mailTo}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while sending email to {mailTo}. Error message: {ex.Message}.");
            }
        }
    }
}
