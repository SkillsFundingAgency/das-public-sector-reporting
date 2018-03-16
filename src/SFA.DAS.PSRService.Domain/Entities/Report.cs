using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SFA.DAS.PSRService.Domain.Entities
{
    using System;
    using Enums;

    public class Report
    {
        public Guid Id { get; set; }

        public string OrganisationName { get; set; }
        public long EmployerId { get; set; }
        
        public bool Submitted { get; set; }
        public string ReportingPeriod { get; set; }
        public Submitted SubmittedDetails { get; set; }
    }
}
