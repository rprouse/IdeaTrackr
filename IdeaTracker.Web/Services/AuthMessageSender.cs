using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.OptionsModel;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdeaTracker.Web.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        AuthMessageSenderOptions _options;

        public AuthMessageSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message, string htmlMessage)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_options.FromName, _options.FromEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            var builder = new BodyBuilder();
            builder.TextBody = message;
            builder.HtmlBody = htmlMessage;

            emailMessage.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                var credentials = new System.Net.NetworkCredential(_options.SmtpUser, _options.SmtpPassword);
                client.LocalDomain = "prouse.org";
                await client.ConnectAsync(_options.SmtpHost, _options.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(credentials);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
