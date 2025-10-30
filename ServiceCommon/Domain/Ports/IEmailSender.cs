namespace ServiceCommon.Domain.Ports;

public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string bodyHtml);
}
