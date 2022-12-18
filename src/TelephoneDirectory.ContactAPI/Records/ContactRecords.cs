namespace TelephoneDirectory.ContactAPI.Records;

public record CreateContact(string Name, string Surname, string Company);

public record GetContact(Guid Id, string Name, string? Surname, DateTime CreatedAt);

public record GetContactDetail(string Name, string? Surname,
    GetContactInformation[]? ContactInformation, DateTime CreatedAt);