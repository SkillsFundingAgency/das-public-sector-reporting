using SFA.DAS.PSRService.Domain.Enums;

namespace SFA.DAS.PSRService.Application.Domain
{
    using System;

    public class GetReportResponse
    {
        public Guid Id { get; set; }
        public long EmployerId { get; set; }
        public string ReportingData { get; set; }
        public string ReportingPeriod { get; set; }
        public bool Submitted { get; set; }
    }
    
}
