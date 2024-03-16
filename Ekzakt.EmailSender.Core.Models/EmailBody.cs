namespace Ekzakt.EmailSender.Core.Models;

public class EmailBody
{
    public string Html { get; set; } = string.Empty;

    public string? Text { get; set; }
}
