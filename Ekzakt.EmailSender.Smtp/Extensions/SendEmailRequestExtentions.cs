using Ekzakt.EmailSender.Core.Models;
using MimeKit;

namespace Ekzakt.EmailSender.Smtp.Extensions;

public static class SendEmailRequestExtentions
{
    /// <summary>
    /// This method converts a SendEmailRequest object toa MimeKit.MimeMessage object.
    /// </summary>
    /// <returns>MimeKit.MimeMessage</returns>
    public static MimeMessage ToMimeMessage(this SendEmailRequest emailRequest)
    {
        MimeMessage mimeMessage = new();

        mimeMessage.Sender = new MailboxAddress(
                emailRequest?.Sender?.Name ?? string.Empty,
                emailRequest?.Sender?.Address);

        mimeMessage.From.Add(mimeMessage.Sender);

        mimeMessage.To.AddRange(emailRequest?.Tos.ToInternetAddressList());
        mimeMessage.Cc.AddRange(emailRequest?.Ccs?.ToInternetAddressList());
        mimeMessage.Bcc.AddRange(emailRequest?.Bccs?.ToInternetAddressList());

        mimeMessage.Subject = emailRequest?.Subject();
        mimeMessage.Body = emailRequest?.ToMimeMessageBody();

        return mimeMessage;
    }



    /// <summary>
    /// This method converts the body of the SendEmailRequest object to a MimeKit.MailEntity object. 
    /// </summary>
    /// <returns>MimeKit.MimeEntity</returns>
    public static MimeEntity ToMimeMessageBody(this SendEmailRequest emailRequest)
    {
        ArgumentException.ThrowIfNullOrEmpty(emailRequest.Body.Html);

        BodyBuilder bodyBuilder = new();

        bodyBuilder.HtmlBody = emailRequest.Body.Html;
        bodyBuilder.TextBody = emailRequest.Body.PlainText ?? string.Empty;

        return bodyBuilder.ToMessageBody();
    }



    /// <summary>
    /// This extention method injects parameters into to the subject of the emailrequest
    /// depending on environment and debug- ore release mode. Nothing will be injects
    /// when the environment is Production end the release-mode Release.
    /// </summary>
    /// <returns>string</returns>
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
