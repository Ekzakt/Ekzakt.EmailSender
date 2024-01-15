using Ekzakt.EmailSender.Core.Contracts;

namespace Ekzakt.EmailSender.Smtp.Configuration;

public class SmtpEmailSenderOptions : IEmailSenderOptions
{
    public const string OptionsName = "SmtpEmail";

    public string FromAddress { get; set; } = string.Empty;

    public string FromDisplayName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Host { get; set; } = string.Empty;

    public int Port { get; set; } = 25;
}
