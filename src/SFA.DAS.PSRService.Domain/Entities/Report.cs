using System;
using System.Collections.Generic;

namespace SFA.DAS.PSRService.Domain.Entities
{
    public class Report
    {
        public Guid Id { get; set; }
        public string OrganisationName { get; set; }
        public string EmployerId { get; set; }
        public IEnumerable<Section> Sections { get; set; }
        public bool Submitted { get; set; }
        public string ReportingPeriod { get; set; }
        public Submitted SubmittedDetails { get; set; }
    }
}
