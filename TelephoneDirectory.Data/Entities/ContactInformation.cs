using System.ComponentModel.DataAnnotations.Schema;
using TelephoneDirectory.Data.Enums;

namespace TelephoneDirectory.Data.Entities;

public class ContactInformation : BaseEntity
{
    public int ContactId { get; set; }
    [ForeignKey("ContactId")] public Contact Contact { get; set; }
    public ContactInformationTypeEnum ContactInformationType { get; set; }
    public string Content { get; set; }
}