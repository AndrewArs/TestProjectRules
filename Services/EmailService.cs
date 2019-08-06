using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Services.Options;

namespace Services
{
    public class EmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly SmtpOptions _smtpOptions;

        public EmailService(IOptions<SmtpOptions> options, ILogger<EmailService> logger)
        {
            _logger = logger;
            _smtpOptions = options.Value;
        }

        public async Task SendHtmlEmail(string recipient, string subject, string body)
        {
            _logger.LogInformation($"Sending message to recipient {recipient}.\nsubject: {subject}\nmessage: {body}");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpOptions.EmailFrom));
            message.To.Add(new MailboxAddress(recipient));

            var builder = new BodyBuilder();
            builder.HtmlBody = body;

            message.Subject = subject;
            message.Body = builder.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, false);
                    await client.AuthenticateAsync(_smtpOptions.Username, _smtpOptions.Password);
                    await client.SendAsync(message);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on sending email: {recipient}");
            }
        }
    }
}
