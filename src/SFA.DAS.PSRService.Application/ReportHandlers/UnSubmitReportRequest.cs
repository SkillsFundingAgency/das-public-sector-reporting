using System;
using MediatR;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class UnSubmitReportRequest : IRequest
    {
        public UnSubmitReportRequest(Report report)
        {
            Report = report ?? throw new ArgumentNullException(nameof(report));
        }

        public Report Report { get; }
    }
}