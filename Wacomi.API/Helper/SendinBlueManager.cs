using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Wacomi.API.Helper
{
    public class SendinBlueManager : IEmailSender
    {
        private AuthMessageSenderOptions _options { get; }
        private readonly ILogger<SendinBlueManager> _logger;
        public SendinBlueManager(IOptions<AuthMessageSenderOptions> optionsAccessor, ILogger<SendinBlueManager> logger)
        {
            this._options = optionsAccessor.Value;
            this._logger = logger;
        }

        public async Task SendEmailAsync(string from, string fromName, string to, string subject, string message)
        {
            //await sendEmailViaAPI(from, fromName, to, subject, message);
            sendEmailViaAPI(from, fromName, to, subject, message);
        }

        private void sendEmailViaAPI(string from, string fromName, string to, string subject, string message)
        {
            mailinblue.API sendinBlue = new mailinblue.API(this._options.ApiKey);
            Dictionary<string, Object> data = new Dictionary<string, Object>();
            Dictionary<string, string> toTemp = new Dictionary<string, string>();
            toTemp.Add(to, "");
            List<string> from_name = new List<string>();
            from_name.Add(from);
            from_name.Add(fromName);

            data.Add("to", toTemp);
            data.Add("from", from_name);
            data.Add("subject", subject);
            data.Add("html", message);

            Object sendEmail = sendinBlue.send_email(data);

        }
    }
}