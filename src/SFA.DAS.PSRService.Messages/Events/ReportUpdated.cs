using System;

namespace SFA.DAS.PSRService.Messages.Events;

public class ReportUpdatedEvent
{
    public Guid Id { get; set; }
    public string EmployerId { get; set; }
    public string ReportingPeriod { get; set; }
    public Answers Answers { get; set; }
    public ReportingPercentages ReportingPercentages { get; set; }
}