# Ekzakt.EmailSender


## Description
This repository contains packages for sending emails via SMTP. It is merely a small and simple wrapper around
the [MimeKit](https://github.com/jstedfast/MimeKit) and [MailKit](https://github.com/jstedfast/MailKit) repos.


## Installation


### 1. Install package
Use the NuGet Package Manager and search for Ekzakt.EmailSender.Smtp, or use the dotnet CLI:
``` C#
dotnet add package Ekzakt.EmailSender.Smtp
```


### 3. Add the following settings to the appsettings.json file.
```json
"Ekzakt": {
  "SmtpEmail": {
    "SenderAddress": "<SENDER_EMAILADDRESS>",
    "SenderDisplayName": "<SENDER_DISPLAYNAME>",
    "UserName": "<SMTP_USERNAME>",
    "Password": "<SMTP_PASSWORD>",
    "Host": "<SMTP_HOSTNAME>",
    "Port": "<SMTP_PORTNUMBER>"
    "UseSSL": <BOOLEAN>
  }
}
```


### 2. Register the class in program.cs


#### 1. Default
``` C#
builder.Services.AddSmtpEmailSender();
```


#### 2. Use a different appsettings.json section name
``` C#
builder.Services.AddSmtpEmailSender(<APPSETTINGS_SECTION_PATH>);
```
where <APPSETTINGS_SECTION_PATH> is the name of the section in your appsettings.json file.
If <APPSETTINGS_SECTION_PATH> is omitted, the default value "SmtpOptions" is used.


#### 3. Use the settings from anywhere you get them:
``` C#
builder.Services.AddSmtpEmailSender(options =>
{
    options.SenderAddress = "<SENDER_EMAILADDRESS>";
    options.SenderDisplayName = "<SENDER_DISPLAYNAME>";
    options.UserName = "<SMTP_USERNAME>";
    options.Password = "<SMTP_PASSWORD>";
    options.Host = "<SMTP_HOSTNAME>";
    options.Port = "<SMTP_PORTNUMBER>";
    options.UseSSL = <BOOLEAN>
});
```


### 3. Usage
``` C#
public class Demo(IEmailSenderService emailSenderService)
{
    private readonly IEmailSenderService _emailSenderService = emailSenderService;

    public async Task<SendEmailResponse> TriggerEmailAsync()
    {
        SendEmailRequest request = new();

        request.Tos.Add(new EmailAddress("johndoe@domain.com", "John Doe"));
        request.Subject = "Send demo email";
        request.Body.Html = "<h1>Html Header</h1><p>Html Body</p>";
        request.Body.PlainText = "Text Body";

        var result = await _emailSenderService.SendAsync(request);

        return result;
    }
}
```


## Authors
- [@ekzakt](https://www.github.com/ekzakt)



## License
[MIT](https://choosealicense.com/licenses/mit/)
