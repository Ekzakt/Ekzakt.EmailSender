using Ekzakt.EmailSender.Core.Models;
using MimeKit;

namespace Ekzakt.EmailSender.Smtp.Extensions;

public static class SendEmailRequestExtentions
{
    public static MimeMessage ToMimeMessage(this SendEmailRequest emailRequest)
    {
        MimeMessage mimeMessage = new();

        mimeMessage.Sender = new MailboxAddress(
                emailRequest?.From?.Name ?? string.Empty,
                emailRequest?.From?.Address);

        mimeMessage.To.AddRange(emailRequest?.Tos.ToInternetAddressList());
        mimeMessage.Cc.AddRange(emailRequest?.Ccs?.ToInternetAddressList());
        mimeMessage.Bcc.AddRange(emailRequest?.Bccs?.ToInternetAddressList());

        mimeMessage.Subject = emailRequest?.Subject();
        mimeMessage.Body = emailRequest?.ToMimeMessageBody();

        return mimeMessage;
    }


    public static MimeEntity ToMimeMessageBody(this SendEmailRequest emailRequest)
    {
        ArgumentException.ThrowIfNullOrEmpty(emailRequest.Body.Html);

        BodyBuilder bodyBuilder = new();

        bodyBuilder.HtmlBody = emailRequest.Body.Html;
        bodyBuilder.TextBody = emailRequest.Body.PlainText ?? string.Empty;

        return bodyBuilder.ToMessageBody();
    }


    public static string Subject(this SendEmailRequest emailRequest)
    {
        bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        var result = isDevelopment
            ? $"*** DEV {emailRequest.Subject} ***"
            : emailRequest.Subject;

        return result;
    }
}
