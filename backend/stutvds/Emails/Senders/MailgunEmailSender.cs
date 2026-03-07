using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using stutvds.Emails.Models;

namespace stutvds.Emails.Senders;

public class MailgunEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;
    private readonly IRazorEmailRenderer _renderer;

    public MailgunEmailSender(IConfiguration configuration, IRazorEmailRenderer renderer)
    {
        _configuration = configuration;
        _renderer = renderer;
    }

    public async Task SendConfirmationEmail(string toEmail, string userId, string token)
    {
        var apiKey = _configuration["Email:Mailgun:ApiKey"];
        var domain = _configuration["Email:Mailgun:Domain"];
        var fromEmail = _configuration["Email:Mailgun:FromEmail"];
        var fromName = _configuration["Email:Mailgun:FromName"];
        var apiBaseUrl = _configuration["Email:Mailgun:ApiBaseUrl"];
        
        var confirmationUrl = $"{_configuration["Email:UrlBase"]}?userId={userId}&token={token}";

        var subject = "Confirm your email";
        var plainText = $"Confirm email: {confirmationUrl}";

        var htmlContent = await _renderer.RenderAsync(
            "stutvds.Emails.Templates.ConfirmationEmail.cshtml",
            new ConfirmationEmailModel
            {
                ConfirmationUrl = confirmationUrl
            });

        using var client = new HttpClient();

        var authToken = Convert.ToBase64String(
            Encoding.ASCII.GetBytes($"api:{apiKey}")
        );

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", authToken);

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("from", $"{fromName} <{fromEmail}>"),
            new KeyValuePair<string, string>("to", toEmail),
            new KeyValuePair<string, string>("subject", subject),
            new KeyValuePair<string, string>("text", plainText),
            new KeyValuePair<string, string>("html", htmlContent)
        });

        var response = await client.PostAsync(
            $"{apiBaseUrl}/{domain}/messages",
            content
        );

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Mailgun failed: {response.StatusCode} - {error}");
        }
    }

    public async Task SendResetPasswordEmail(string toEmail, string userId, string token)
    {
        var apiKey = _configuration["Email:Mailgun:ApiKey"];
        var domain = _configuration["Email:Mailgun:Domain"];
        var fromEmail = _configuration["Email:Mailgun:FromEmail"];
        var fromName = _configuration["Email:Mailgun:FromName"];
        var apiBaseUrl = _configuration["Email:Mailgun:ApiBaseUrl"];
        
        var resetPasswordUrl = $"{_configuration["Email:UrlBase"]}?userId={userId}&token={token}";

        var subject = "Reset password";
        var plainText = $"Reset password: {resetPasswordUrl}";

        var htmlContent = await _renderer.RenderAsync(
            "stutvds.Emails.Templates.ResetPasswordEmail.cshtml",
            new ResetPasswordEmailModel
            {
                ResetPasswordUrl = resetPasswordUrl
            });

        using var client = new HttpClient();

        var authToken = Convert.ToBase64String(
            Encoding.ASCII.GetBytes($"api:{apiKey}")
        );

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", authToken);

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("from", $"{fromName} <{fromEmail}>"),
            new KeyValuePair<string, string>("to", toEmail),
            new KeyValuePair<string, string>("subject", subject),
            new KeyValuePair<string, string>("text", plainText),
            new KeyValuePair<string, string>("html", htmlContent)
        });

        var response = await client.PostAsync(
            $"{apiBaseUrl}/{domain}/messages",
            content
        );

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Mailgun failed: {response.StatusCode} - {error}");
        }
    }
}