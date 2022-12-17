namespace TelephoneDirectory.Data.Entities;

public class Contact : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Company { get; set; }
}