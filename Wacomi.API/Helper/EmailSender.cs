using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Wacomi.API.Helper
{
    public class EmailSender : IEmailSender
    {
        public AuthMessageSenderOptions Options { get; }
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public Task SendEmailAsync(string from, string fromName, string to, string subject, string message)
        {
            return Execute(Options, subject, message, from, fromName, to);
        }

        public async Task Execute(AuthMessageSenderOptions options, string subject, string message, string from, string fromName, string to)
        {
            using (SmtpClient smtp = new SmtpClient(options.SMTPServer)
            {
                EnableSsl = true,
                Port = options.SMTPPort,
                Credentials = new NetworkCredential(options.UserName, options.Password)
            }
            )
            {
                MailMessage mail = new MailMessage()
                {
                    //From = new MailAddress("wacomi_test@wacomi.a2hosted.com", "Wacomi")
                    From = new MailAddress(from, fromName)
                };
                mail.To.Add(new MailAddress(to));

                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.Normal;

                await smtp.SendMailAsync(mail);
            }
        }
    }
}