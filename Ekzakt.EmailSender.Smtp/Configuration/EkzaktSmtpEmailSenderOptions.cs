﻿using Ekzakt.EmailSender.Core.Contracts;

namespace Ekzakt.EmailSender.Smtp.Configuration;

[Obsolete("Use EkzaktEmailSenderSmtpOptions instead. This class will be deleted in a future version.")]
public class EkzaktSmtpEmailSenderOptions : IEmailSenderOptions
{
    public const string OptionsName = "Ekzakt:SmtpEmail";

    [Obsolete("Use SenderAddress instead.  FromAddress wil be removed in future versions.")]
    public string FromAddress { get; set; } = string.Empty;

    [Obsolete("Use SenderDisplayName instead. FromDisplayName wil be removed in future versions.")]
    public string FromDisplayName { get; set; } = string.Empty;

    public string SenderAddress { get; set; } = string.Empty;

    public string SenderDisplayName { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Host { get; set; } = string.Empty;

    public int Port { get; set; } = 587;

    public bool UseSSL { get; set; } = true;

}
