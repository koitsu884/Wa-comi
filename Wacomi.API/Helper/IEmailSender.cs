using System.Threading.Tasks;

namespace Wacomi.API.Helper
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}