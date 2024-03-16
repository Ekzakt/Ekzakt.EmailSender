using Ekzakt.EmailSender.Core.Contracts;
using Ekzakt.EmailSender.Core.Models;
using Ekzakt.EmailSender.Core.Models.Requests;
using Ekzakt.EmailSender.Core.Validators;
using Ekzakt.EmailSender.Smtp.Services;
using Ekzakt.EmailSender.Smtp.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Ekzakt.EmailSender.Smtp.Configuration;

public static class DepencyInjection
{
    [Obsolete("Use AddEkzaktSmtpEmailSender instead. This method will be removed in a future versions.")]
    public static IServiceCollection AddSmtpEmailSender(this IServiceCollection services, Action<SmtpEmailSenderOptions> options)
    {
        services.Configure(options);

        services.AddSmtpEmailSender();

        return services;
    }


    [Obsolete("Use AddEkzaktSmtpEmailSender instead. This method will be removed in a future versions.")]
    public static IServiceCollection AddSmtpEmailSender(this IServiceCollection services, string? configSectionPath = null)
    {
        configSectionPath ??= EkzaktSmtpEmailSenderOptions.OptionsName;

        services
            .AddOptions<SmtpEmailSenderOptions>()
            .BindConfiguration(configSectionPath);

        services.AddSmtpEmailSender(); 
        
        return services;
    }


    public static IServiceCollection AddEkzaktSmtpEmailSender(this IServiceCollection services, Action<EkzaktSmtpEmailSenderOptions> options)
    {
        services.Configure(options);

        services.AddEkzaktSmtpEmailSender();

        return services;
    }


     public static IServiceCollection AddEkzaktSmtpEmailSender(this IServiceCollection services, string? configSectionPath = null)
    {
        configSectionPath ??= EkzaktSmtpEmailSenderOptions.OptionsName;

        services
            .AddOptions<EkzaktSmtpEmailSenderOptions>()
            .BindConfiguration(configSectionPath);

        services.AddEkzaktSmtpEmailSender();

        return services;
    }


    #region Helpers

    private static IServiceCollection AddEkzaktSmtpEmailSender(this IServiceCollection services)
    {
        services.AddScoped<IValidator<EkzaktSmtpEmailSenderOptions>, EkzaktSmtpEmailSenderOptionsValidator>();
        services.AddScoped<IValidator<SendEmailRequest>, SendEmailRequestValidator>();
        services.AddScoped<IValidator<EmailBody>, EmailBodyValidator>();
        services.AddScoped<IValidator<EmailAddress>, EmailAddressValidator>();

        services.AddScoped<IEkzaktEmailSenderService, EkzaktSmtpEmailSenderService>();

        return services;
    }

    #endregion Helpers
}
