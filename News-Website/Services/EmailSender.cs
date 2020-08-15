using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace RPGMegaMart.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor, IConfiguration config)
        {
            //Options = optionsAccessor.Value;
            Options = new AuthMessageSenderOptions(config);
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            Response response = await Execute(Options.SendGridKey, subject, message, email);
            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                // log or throw
                throw new Exception("Could not send email: " + await response.Body.ReadAsStringAsync());
            }
        }

        public async Task<Response> Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("account@rpgmegamart.com", Options.SendGridUser),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return await client.SendEmailAsync(msg);
        }
    }
}