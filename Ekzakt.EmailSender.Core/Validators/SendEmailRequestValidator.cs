using Ekzakt.EmailSender.Core.Models.Requests;
using FluentValidation;

namespace Ekzakt.EmailSender.Core.Validators;

public class SendEmailRequestValidator : AbstractValidator<SendEmailRequest>
{
    public SendEmailRequestValidator()
    {
        RuleFor(x => x.TemplateName)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.RecipientType)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Email.Sender)
            .SetValidator(new EmailAddressValidator());

        RuleFor(x => x.Email.Tos.Count)
            .GreaterThanOrEqualTo(1)
            .WithMessage("At least one destination address (Tos) must be set.");

        RuleForEach(x => x.Email.Tos)
            .NotNull()
            .NotEmpty()
            .SetValidator(new EmailAddressValidator());

        RuleForEach(x => x.Email.Ccs)
            .SetValidator(new EmailAddressValidator());

        RuleForEach(x => x.Email.Bccs)
            .SetValidator(new EmailAddressValidator());

        RuleFor(x => x.Email.Subject)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Email.Body)
            .SetValidator(new EmailBodyValidator());
    }
}
