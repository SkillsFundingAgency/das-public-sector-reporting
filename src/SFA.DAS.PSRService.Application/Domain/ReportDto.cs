using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.PSRService.Application.Domain
{
    public class ReportDto
    {
        public Guid Id { get; set; }
        public long EmployerId { get; set; }
        public string ReportingData { get; set; }
        public string ReportingPeriod { get; set; }
        public bool Submitted { get; set; }
    }
}
