using System;

namespace SFA.DAS.PSRService.Application.ReportHandlers;

public class GetReportResponse
{
    public Guid Id { get; set; }
    public long EmployerId { get; set; }
    public string ReportingData { get; set; }
    public string ReportingPeriod { get; set; }
    public bool Submitted { get; set; }
}