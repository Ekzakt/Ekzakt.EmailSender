using Ekzakt.EmailSender.Core.Models;
using MimeKit;

namespace Ekzakt.EmailSender.Smtp.Extensions;

public static class EmailAddressListExtensions
{
    public static InternetAddressList ToInternetAddressList(this List<EmailAddress> emailAddressList)
    {
        InternetAddressList addressesList = new();

        foreach (var emailAddress in emailAddressList ?? new List<EmailAddress>())
        {
            addressesList.Add(new MailboxAddress(emailAddress.Name ?? string.Empty, emailAddress.Address));
        }

        return addressesList;
    }
}
