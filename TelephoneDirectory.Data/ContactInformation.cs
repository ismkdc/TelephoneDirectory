using TelephoneDirectory.Data.Enums;

namespace TelephoneDirectory.Data;

public class ContactInformation: BaseEntity
{
    public ContactInformationTypeEnum ContactInformationType { get; set; }
    public string Content { get; set; }
}