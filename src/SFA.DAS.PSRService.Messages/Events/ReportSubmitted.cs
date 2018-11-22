using System;
using SFA.DAS.NServiceBus;

namespace SFA.DAS.PSRService.Messages.Events
{
    public class ReportSubmitted
    :Event
    {
        public Guid Id { get; set; }
        public string EmployerId { get; set; }
        public string ReportingPeriod { get; set; }
        public Submitter Submitter { get; set; }
        public Answers Answers { get; set; }
        public ReportingPercentages ReportingPercentages { get; set; }
    }
}