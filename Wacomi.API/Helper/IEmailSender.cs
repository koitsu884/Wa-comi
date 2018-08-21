using System.Threading.Tasks;

namespace Wacomi.API.Helper
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string from, string fromName, string to, string subject, string message);
    }
}