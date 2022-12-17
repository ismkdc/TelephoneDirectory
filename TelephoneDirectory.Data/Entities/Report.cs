using System.ComponentModel.DataAnnotations.Schema;
using TelephoneDirectory.Data.Enums;

namespace TelephoneDirectory.Data.Entities;

public class Report : BaseEntity
{
    [Column(TypeName = "jsonb")] public ReportContent[]? Content { get; set; }

    public ReportStatusEnum ReportStatus { get; set; }
}