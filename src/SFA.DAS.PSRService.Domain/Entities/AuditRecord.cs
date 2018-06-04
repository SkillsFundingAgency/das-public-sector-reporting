using System;
using System.Collections.Generic;

namespace SFA.DAS.PSRService.Domain.Entities
{
    public class AuditRecord
    {
        public DateTime UpdatedUtc { get; set; }
        public IEnumerable<Section> Sections { get; set; }
        public User UpdatedBy { get; set; }
        public ReportingPercentages ReportingPercentages { get; set; }
    }
}
