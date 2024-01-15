namespace Ekzakt.EmailSender.Core.Models;

public class EmailAddress
{
    public EmailAddress(string address)
    {
        Address = address;
    }

    public EmailAddress(string address, string name)
    {
        Address = address;
        Name = name;
    }


    public string Address { get; private set; } = string.Empty;
    public string? Name { get; private set; } = string.Empty;

}