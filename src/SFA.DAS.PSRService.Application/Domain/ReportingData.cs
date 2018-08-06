using System.Collections.Generic;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.Domain
{
    public class ReportingData
    {
    
        public IEnumerable<Section> Questions { get; set; }
        public string OrganisationName { get; set; }
        public Submitted Submitted { get; set; }
        public ReportingPercentages ReportingPercentages {get; set; }
    }
}
