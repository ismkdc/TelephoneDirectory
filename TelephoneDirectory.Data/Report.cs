using TelephoneDirectory.Data.Enums;

namespace TelephoneDirectory.Data;

public class Report : BaseEntity
{
    public ReportStatusEnum ReportStatus { get; set; }
}