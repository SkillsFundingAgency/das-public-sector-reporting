using System;
using System.Collections.Generic;

namespace SFA.DAS.PSRService.Domain.Entities
{
    public class AuditRecord
    {
        private TimeZoneInfo _ukDaylightSavingAwareTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

        public DateTime UpdatedUtc { get; set; }
        public IEnumerable<Section> Sections { get; set; }
        public User UpdatedBy { get; set; }
        public ReportingPercentages ReportingPercentages { get; set; }
        public ReportingPercentages ReportingPercentagesSchools { get; set; }
        public string OrganisationName { get; set; }
        public string SerialNo { get; set; }
        public bool? HasMinimumEmployeeHeadcount { get; set; }
        public int TotalEmployees { get; set; }
        public bool? IsLocalAuthority { get; set; }

        public DateTime UpdatedLocal => TimeZoneInfo
            .ConvertTimeFromUtc(
                UpdatedUtc,
                _ukDaylightSavingAwareTimeZone);
    }
}
