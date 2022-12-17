﻿using TelephoneDirectory.Data.Entities;
using TelephoneDirectory.Data.Enums;

namespace TelephoneDirectory.WebAPI.Records;

public record GetReport(Guid Id, ReportStatusEnum ReportStatus, DateTime CreatedAt);

public record GetReportDetail(ReportContent Content, ReportStatusEnum ReportStatus, DateTime CreatedAt);