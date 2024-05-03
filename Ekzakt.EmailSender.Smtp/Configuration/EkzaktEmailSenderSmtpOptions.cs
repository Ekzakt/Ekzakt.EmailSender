namespace Ekzakt.EmailSender.Smtp.Configuration;

public class EkzaktEmailSenderSmtpOptions
{
    public const string OptionsName = "Ekzakt:SmtpEmail";

    public string SenderAddress { get; set; } = string.Empty;

    public string SenderDisplayName { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Host { get; set; } = string.Empty;

    public int Port { get; set; } = 587;

    public bool UseSSL { get; set; } = true;

}