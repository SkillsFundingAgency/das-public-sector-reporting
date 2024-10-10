using System;

namespace SFA.DAS.PSRService.Messages.Events;

public class ReportCreatedEvent
{
    public Guid Id { get; set; }
    public string EmployerId { get; set; }
    public string ReportingPeriod { get; set; }
}