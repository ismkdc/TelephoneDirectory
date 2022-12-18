using TelephoneDirectory.Data.Enums;

namespace TelephoneDirectory.ReportService.Records;

public record GetReport(Guid Id, string FilePath, ReportStatusEnum ReportStatus, DateTime CreatedAt);