namespace TelephoneDirectory.WebAPI.Records;

public record CreateContact(string Name, string Surname, string Company);
public record GetContact(Guid Id, string Name, string? Surname);
public record GetContactDetail(Guid Id, string Name, string? Surname,
    GetContactInformation[]? ContactInformation);