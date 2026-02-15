using System.Threading.Tasks;

namespace stutvds.Emails;

public interface IEmailSender
{
    Task SendConfirmationEmail(string toEmail, string userId, string token);
    Task SendResetPasswordEmail(string toEmail, string userId, string token);
}