using TelephoneDirectory.Data.Enums;

namespace TelephoneDirectory.ReportAPI.Records;

public record GetReport(Guid Id, string FilePath, ReportStatusEnum ReportStatus, DateTime CreatedAt);