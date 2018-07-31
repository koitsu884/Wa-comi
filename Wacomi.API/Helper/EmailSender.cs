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

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options, subject, message, email);
        }

        public async Task Execute(AuthMessageSenderOptions options, string subject, string message, string email)
        {
            using (SmtpClient smtp = new SmtpClient(options.SMTPServer){
                EnableSsl = false,
                Credentials = new NetworkCredential(options.UserName, options.Password)
                }
            )
            {
                MailMessage mail = new MailMessage()
                {
                    //From = new MailAddress("wacomi_test@wacomi.a2hosted.com", "Wacomi")
                    From = new MailAddress("wacomi_test@wacomi.virtuozzo.co.nz", "Wacomi")
                };
                mail.To.Add(new MailAddress(email));

                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.Normal;

                await smtp.SendMailAsync(mail);
            }

        }
    }
}