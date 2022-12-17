namespace TelephoneDirectory.Data.Entities;

public class Contact : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Company { get; set; }
    public IList<ContactInformation> ContactInformation { get; set; }

}