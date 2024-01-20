using Ekzakt.EmailSender.Core.Models;
using FluentValidation;

namespace Ekzakt.EmailSender.Core.Validators;

public class EmailAddressValidator : AbstractValidator<EmailAddress>
{
    public EmailAddressValidator()
    {
        RuleFor(x => x.Address)
            .EmailAddress();
    }
}
