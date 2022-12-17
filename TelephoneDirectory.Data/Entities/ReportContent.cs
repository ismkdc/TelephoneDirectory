namespace TelephoneDirectory.Data.Entities;

public class ReportContent
{
    public string Location { get; set; }
    public IEnumerable<ReportContentDetail> Contacts { get; set; }
}

public class ReportContentDetail
{
    public string Name { get; set; }
    public string Surname { get; set; }
}