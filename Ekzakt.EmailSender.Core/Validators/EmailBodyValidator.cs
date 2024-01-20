using Ekzakt.EmailSender.Core.Models;
using FluentValidation;

namespace Ekzakt.EmailSender.Core.Validators;

public class EmailBodyValidator : AbstractValidator<EmailBody>
{
    public EmailBodyValidator()
    {
        RuleFor(x => x.Html)
            .NotEmpty()
            .NotNull();

    }
}
