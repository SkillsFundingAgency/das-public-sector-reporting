using System;
using MediatR;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class UpdateReportRequest : IRequest
    {
        public UpdateReportRequest()
        {
            AuditWindowSize = TimeSpan.FromMinutes(5); // default
        }

        public Report Report { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public TimeSpan AuditWindowSize { get; set; }
    }
}
