namespace TelephoneDirectory.WebAPI.Models;

public class ContactInformation
{
    public Guid Id { get; set; }
    public ContactInformationType ContactInformationType { get; set; }
    public string Content { get; set; }
}