using System.Net;
using System.Net.Mail;

namespace fit.Services;

public interface IEmailSender
{
    Task SendAsync(string toEmail, string subject, string htmlBody);
}

public sealed class SmtpEmailSender(IConfiguration config) : IEmailSender
{
    public async Task SendAsync(string toEmail, string subject, string htmlBody)
    {
        var host = config["Smtp:Host"];
        var portStr = config["Smtp:Port"];
        var username = config["Smtp:Username"];
        var password = config["Smtp:Password"];
        var fromEmail = config["Smtp:FromEmail"];
        var fromName = config["Smtp:FromName"] ?? "FitTrack";

        if (string.IsNullOrWhiteSpace(host) ||
            string.IsNullOrWhiteSpace(portStr) ||
            string.IsNullOrWhiteSpace(fromEmail))
        {
            throw new InvalidOperationException("SMTP is not configured. Add Smtp settings to appsettings.json.");
        }

        if (!int.TryParse(portStr, out var port))
            throw new InvalidOperationException("Smtp:Port must be an integer.");

        using var msg = new MailMessage();
        msg.From = new MailAddress(fromEmail, fromName);
        msg.To.Add(new MailAddress(toEmail));
        msg.Subject = subject;
        msg.Body = htmlBody;
        msg.IsBodyHtml = true;

        using var client = new SmtpClient(host, port);
        var enableSsl = string.Equals(config["Smtp:EnableSsl"], "true", StringComparison.OrdinalIgnoreCase);
        client.EnableSsl = enableSsl;

        if (!string.IsNullOrWhiteSpace(username))
        {
            client.Credentials = new NetworkCredential(username, password);
        }
        else
        {
            client.UseDefaultCredentials = true;
        }

        await client.SendMailAsync(msg);
    }
}

