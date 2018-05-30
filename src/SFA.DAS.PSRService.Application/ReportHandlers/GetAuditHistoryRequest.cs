using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class GetAuditHistoryRequest : IRequest<IEnumerable<AuditRecord>>
    {
        public Report Report { get; }

        public GetAuditHistoryRequest(Report report)
        {
            this.Report = report ?? throw new ArgumentNullException(nameof(report));
        }
    }
}