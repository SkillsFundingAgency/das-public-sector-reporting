using System;
using System.Collections.Generic;

namespace SFA.DAS.PSRService.Domain.Entities
{
    public class AuditRecord
    {
        public User UpdatedBy { get; }
        public DateTime UpdatedUtc { get; }
        public DateTime UpdatedLocal { get; }
        public IEnumerable<Section> Sections { get; }
        public ReportingPercentages ReportingPercentages { get; }
        public string OrganisationName { get; }

        public AuditRecord(string organisationName, IEnumerable<Section> sections, ReportingPercentages reportingPercentages, User updatedBy, DateTime updatedUtc)
        {
            OrganisationName = organisationName;
            Sections = sections;
            ReportingPercentages = reportingPercentages;
            UpdatedBy = updatedBy;

            UpdatedUtc = DateTime.SpecifyKind(updatedUtc, DateTimeKind.Utc);

            UpdatedLocal = DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(
                UpdatedUtc,
                TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time")),
                DateTimeKind.Local);
        }
    }
}
