using TelephoneDirectory.Data.Enums;

namespace TelephoneDirectory.WebAPI.Records;

public record GetContactInformation(Guid Id, ContactInformationTypeEnum ContactInformationType, string Content);