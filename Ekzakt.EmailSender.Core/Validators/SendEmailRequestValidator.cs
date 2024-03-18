﻿using Ekzakt.EmailSender.Core.Models.Requests;
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

        RuleFor(x => x.Email)
            .SetValidator(new EmailValidator());
    }
}
