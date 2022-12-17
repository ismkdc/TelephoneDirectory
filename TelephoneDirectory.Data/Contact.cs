namespace TelephoneDirectory.Data;

public class Contact : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Company { get; set; }
}