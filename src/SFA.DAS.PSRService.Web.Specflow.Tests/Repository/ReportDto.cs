using System;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository
{
    public class ReportDto
    {
        public Guid Id { get; set; }
        public string EmployerId { get; set; }
        public string ReportingData { get; set; }
        public string ReportingPeriod { get; set; }
        public bool Submitted { get; set; }
        public DateTime? AuditWindowStartUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public string UpdatedBy { get; set; }
    }
}
