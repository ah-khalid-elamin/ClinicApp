using System.Threading.Tasks;

namespace Bot.Helpers.Mail
{
    public interface IEmailService
    {
        public Task SendEmail(string toEmail, string subject, string messageBody);
    }
}
