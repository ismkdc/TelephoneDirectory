using TelephoneDirectory.Data.Enums;

namespace TelephoneDirectory.ContactAPI.Records;

public record GetContactInformation(Guid Id, ContactInformationTypeEnum ContactInformationType, string Content,
    DateTime CreatedAt);

public record CreateContactInformation(Guid ContactId, ContactInformationTypeEnum ContactInformationType,
    string Content);