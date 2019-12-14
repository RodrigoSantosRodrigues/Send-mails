using System.Threading.Tasks;

namespace SendEmail.Services
{
    public interface IEmailService
    {
        Task SendMail(string email, string subject, string message);
    }
}
