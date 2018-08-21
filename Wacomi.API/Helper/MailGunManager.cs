using System;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Wacomi.API.Helper
{
    public class MailGunManager : IEmailSender
    {
        private AuthMessageSenderOptions _options { get; }
        private readonly ILogger<MailGunManager> _logger;
        public MailGunManager(IOptions<AuthMessageSenderOptions> optionsAccessor, ILogger<MailGunManager> logger)
        {
            this._options = optionsAccessor.Value;
            this._logger = logger;
        }
        public async Task SendEmailAsync(string from, string fromName, string to, string subject, string message)
        {
            await sendEmailViaSMTP(from, fromName, to, subject, message);
        }

        private async Task sendEmailViaSMTP(string from, string fromName, string to, string subject, string message)
        {

            var apiKey = _options.ApiKey;
            MailMessage mailMsg = new MailMessage();
            mailMsg.To.Add(new MailAddress(to));
            mailMsg.From = new MailAddress(from, fromName);

            mailMsg.Subject = subject;
            string text = message;
            string html = message;
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            // Init SmtpClient and send
            using (SmtpClient smtpClient = new SmtpClient("smtp.mailgun.org", System.Convert.ToInt32(587)))
            {
                try
                {
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("apikey", apiKey);
                    smtpClient.Credentials = credentials;

                    await smtpClient.SendMailAsync(mailMsg);
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex.Message);
                }
            }
        }
    }
}