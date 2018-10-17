using System;
using SFA.DAS.PSRService.Messages.Events;

namespace SFA.DAS.PSRService.Messages
{
    public class ReportMessage
    {
        public Guid Id { get; set; }
        public string OrganisationName { get; set; }
        public string EmployerId { get; set; }
        public ReportStatus Status { get; set; }
        public bool Submitted { get; set; }
        public string ReportingPeriod { get; set; }
        public Submitted SubmittedDetails { get; set; }
        public ReportingPercentages ReportingPercentages { get; set; }
        public DateTime? UpdatedUtc { get; set; }
    }
}
