namespace Ekzakt.EmailSender.Core.Models;

public class EmailBody
{
    public string Html { get; set; } = string.Empty;

    [Obsolete("Use Text instead.  Plaintext will be removed in future versions.")]
    public string PlainText { get; set; } = string.Empty;

    public string? Text { get; set; }
}
