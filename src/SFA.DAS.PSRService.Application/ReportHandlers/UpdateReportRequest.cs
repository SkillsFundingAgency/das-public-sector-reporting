using System;
using MediatR;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class UpdateReportRequest : IRequest
    {
        public UpdateReportRequest(Report report, User user, bool? isLocalAuthority)
        {
            Report = report ?? throw new ArgumentNullException(nameof(report));

            User = user ?? throw new ArgumentNullException(nameof(user));

            AuditWindowSize = TimeSpan.FromMinutes(5); // default

            IsLocalAuthority = isLocalAuthority;
        }

        public Report Report { get; set; }
        public TimeSpan AuditWindowSize { get; set; }
        public User User { get; }
        public bool? IsLocalAuthority { get; }
    }
}
