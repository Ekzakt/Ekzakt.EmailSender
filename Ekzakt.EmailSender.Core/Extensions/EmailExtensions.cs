using Ekzakt.EmailSender.Core.Models;
using Ekzakt.Utilities;

namespace Ekzakt.EmailSender.Core.Extensions;

public static class EmailExtensions
{
    public static Email? ApplyReplacements(this Email email, StringReplacer replacer)
    {
        if (email is null)
        {
            return null;
        }
         
        email.Subject = replacer.Replace(email!.Subject ?? string.Empty);
        email.Body.Html = replacer.Replace(email!.Body.Html);
        email.Body.Text = replacer.Replace(email!.Body.Text ?? string.Empty);
        
        return email;
    }
}
