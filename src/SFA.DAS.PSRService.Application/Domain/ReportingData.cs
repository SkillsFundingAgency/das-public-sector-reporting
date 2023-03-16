using System.Collections.Generic;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.Domain
{
    public class ReportingData
    {
        public IEnumerable<Section> Questions { get; set; }
        public string OrganisationName { get; set; }
        public bool? HasMinimumEmployeeHeadcount { get; set; }
        public int TotalEmployees { get; set; }
        public bool? IsLocalAuthority { get; set; }
        public string SerialNo { get; set; }
        public Submitted Submitted { get; set; }
        public ReportingPercentages ReportingPercentages { get; set; }
        public ReportingPercentages ReportingPercentagesSchools { get; set; }
    }
}
