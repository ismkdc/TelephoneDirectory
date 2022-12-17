using System.ComponentModel.DataAnnotations.Schema;

namespace TelephoneDirectory.Data.Entities;

public class ReportDetail : BaseEntity
{
    public Report Report { get; set; }
    [Column(TypeName = "jsonb")]
    public ReportContent Content { get; set; }
}