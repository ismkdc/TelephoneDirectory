namespace TelephoneDirectory.Data.Entities;

public class ReportContent
{
    public string Location { get; set; }
    public (string name, string surname)[] Contacts { get; set; }
}