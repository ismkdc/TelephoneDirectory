using TelephoneDirectory.Data.Enums;

namespace TelephoneDirectory.Data.Entities;

public class Report : BaseEntity
{
    public string? FilePath { get; set; }

    public ReportStatusEnum ReportStatus { get; set; }
}