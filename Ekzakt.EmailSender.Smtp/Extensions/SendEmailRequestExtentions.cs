using Ekzakt.EmailSender.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;

namespace Ekzakt.EmailSender.Smtp.Extensions;

public static class SendEmailRequestExtentions
{
    public static MimeMessage ToMimeMessage(this SendEmailRequest emailRequest)
    {
        MimeMessage mimeMessage = new();

        mimeMessage.Sender = new MailboxAddress(
                emailRequest?.Sender?.Name ?? string.Empty,
                emailRequest?.Sender?.Address);

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
        bool isStaging = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Staging";

        bool isDebug = false;

#if DEBUG
        isDebug = true;
#endif

        var subject = emailRequest.Subject;
        var status = string.Empty;

        List<string> statesList = [];

        if (isDebug)
        {
            statesList.Add("DEBUG");
        }

        if (isDevelopment)
        {
            statesList.Add("DEV");
        }

        if (isStaging)
        {
            statesList.Add("STAGE");
        }

        var states = string.Join(" - ", statesList);

        if (statesList.Count > 0)
        {
            return $"*** {states} *** {subject}";
        }

        return subject;
    }
}
