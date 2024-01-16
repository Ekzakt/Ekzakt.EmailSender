# Ekzakt.EmailSender
Packages for sending emails via an smtp-server and is merely a small wrapper around
the [MimeKit](https://github.com/jstedfast/MimeKit) and [MailKit](https://github.com/jstedfast/MailKit) repo.


## Installation

### 1. Install package
Use the NuGet Package Manager and search for Ekzakt.EmailSender.Smtp, or use the dotnet CLI:
``` C#
dotnet add package Ekzakt.EmailSender.Smtp
```

### 3. Add the following code in the appsettings.json file.
```json
"SmtpEmail": {
    "FromAddress": "<YOUR_SENDER_EMAIL_ADDRESS>",
    "FromDisplayName": "<YOUR_SENDER_NAME>",
    "UserName": "<SMTP_USERNAME>",
    "Password": "<SMTP_PASSWORD>",
    "Host": "<SMTP_SERVER_NAME>",
    "Port": "<SMTP_PORT_NUMBER>"
}
```

### 2. Register the class in program.cs


#### 1. Default setting
``` C#
builder.Services.AddSmtpEmailSender();
```


#### 2. Use a different secion name
``` C#
builder.Services.AddSmtpEmailSender(<APPSETTINGS_SECTION_PATH>);
```
where <APPSETTINGS_SECTION_PATH> is the name of the section in your appsettings.json file.
If <APPSETTINGS_SECTION_PATH> is omitted, the default value "SmtpOptions" is used.


#### 3. Use the settings from anywhere you get them:
``` C#
builder.Services.AddSmtpEmailSender(options =>
{
    options.FromAddress = "<YOUR_SENDER_EMAIL_ADDRESS>";
    options.FromDisplayName = "<YOUR_SENDER_NAME>";
    options.UserName = "<SMTP_USERNAME>";
    options.Password = "<SMTP_PASSWORD>";
    options.Host = "<SMTP_HOST_NAME>";
    options.Port = <SMTP_PORT_NUMER>;
});
```


### 4. Usage

``` C#
public class Demo(IEmailSenderService emailSenderService)
{
    private readonly IEmailSenderService _emailSenderService = emailSenderService;

    public async Task<SendEmailResponse> TriggerEmailAsync()
    {
        SendEmailRequest request = new();

        request.Tos.Add(new EmailAddress("johndoe@domain.com, "John Doe"));
        request.Subject = "Send demo email";
        request.HtmlBody = "<h1>HtmlBody</h1><p>HtmlBody</p>";
        request.TextBody = "TextBody";

        var result = await _emailSenderService.SendAsync(request);

        result;
    }
}
```


## Authors
- [@ekzakt](https://www.github.com/ekzakt)



## License
[MIT](https://choosealicense.com/licenses/mit/)
