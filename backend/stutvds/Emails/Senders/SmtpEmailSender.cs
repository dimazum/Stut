using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using stutvds.Emails.Models;

namespace stutvds.Emails.Senders;

public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;
    private readonly IRazorEmailRenderer _renderer;

    public SmtpEmailSender(IConfiguration configuration, IRazorEmailRenderer renderer)
    {
        _configuration = configuration;
        _renderer = renderer;
    }

    public async Task SendConfirmationEmail(string toEmail, string userId, string token)
    {
        var confirmationUrl = $"{_configuration["Email:UrlBase"]}/confirm-email/?userId={userId}&token={token}";
        
        var html = await _renderer.RenderAsync(
            "stutvds.Emails.Templates.ConfirmationEmail.cshtml",
            new ConfirmationEmailModel
            {
                ConfirmationUrl = confirmationUrl
            });

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_configuration["Email:SmtpLocal:FromEmail"]));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = "Confirm your email";
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = html
        };
        
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_configuration["Email:SmtpLocal:Host"], int.Parse(_configuration["Email:SmtpLocal:Port"]!));
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
    
    public async Task SendResetPasswordEmail(string toEmail, string userId, string token)
    {
        var resetPasswordUrl = $"{_configuration["Email:UrlBase"]}/reset-password?userId={userId}&token={token}";
        
        var html = await _renderer.RenderAsync(
            "stutvds.Emails.Templates.ResetPasswordEmail.cshtml",
            new ResetPasswordEmailModel
            {
                ResetPasswordUrl = resetPasswordUrl
            });

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_configuration["Email:SmtpLocal:FromEmail"]));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = "Reset password";
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = html
        };
        
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_configuration["Email:SmtpLocal:Host"], int.Parse(_configuration["Email:SmtpLocal:Port"]!));
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}