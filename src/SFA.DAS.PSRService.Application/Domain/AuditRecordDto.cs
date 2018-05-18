using System;

namespace SFA.DAS.PSRService.Application.Domain
{
    public class AuditRecordDto
    {
        public int Id { get;set; }
        public Guid ReportId { get;set; }
        public DateTime UpdatedUtc{get; set; }
        public string ReportingData { get; set; }
        public string UpdatedBy { get; set; }
    }
}
