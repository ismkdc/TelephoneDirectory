using TelephoneDirectory.Data.Enums;

namespace TelephoneDirectory.Data.Entities;

public class Report : BaseEntity
{
    public ReportStatusEnum ReportStatus { get; set; }
}