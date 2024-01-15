# Ekzakt.EmailSender.Core
Package for implementing [Ekzakt.EmailSender.Smtp](https://github.com/Ekzakt/Ekzakt/tree/master/Ekzakt.EmailSender.Smtp) repo.

## Installation

### 1. Install package
Use the NuGet Package Manager and search for Ekzakt.EmailSender.Core, or use the dotnet CLI:
``` C#
dotnet add package Ekzakt.EmailSender.Core
```

### 2. Register the class in startup.cs
``` C#
builder.Services.AddScoped<IEmailSenderService, SmtpEmailSenderService>();
```

## Usage
Import the namespace Ekzakt.EmailSender.Core in every class you want to use it.
``` C#
import Ekzakt.EmailSender.Smtp
```

## License
[MIT](https://choosealicense.com/licenses/mit/)