using Ekzakt.EmailSender.Core.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Ekzakt.EmailSender.Smtp.Configuration;

public class SmtpEmailSenderOptions : IEmailSenderOptions
{
    public const string OptionsName = "SmtpEmail";

    [Required]
    [EmailAddress]
    public string FromAddress { get; set; } = string.Empty;

    [Required]
    public string FromDisplayName { get; set; } = string.Empty;

    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Host { get; set; } = string.Empty;

    [Required]
    [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535")]
    public int Port { get; set; } = 587;

    public bool UseSSL { get; set; } = true;
}
