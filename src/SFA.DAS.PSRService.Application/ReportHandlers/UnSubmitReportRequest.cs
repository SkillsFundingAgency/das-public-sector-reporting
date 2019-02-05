using System;
using MediatR;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class UnSubmitReportRequest : IRequest
    {
        public UnSubmitReportRequest(string employerAccountId, Period reportingPeriod)
        {
            if (string.IsNullOrWhiteSpace(employerAccountId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(employerAccountId));

            EmployerAccountId = employerAccountId;
            ReportingPeriod = reportingPeriod ?? throw new ArgumentNullException(nameof(reportingPeriod));
        }

        public Period ReportingPeriod { get; }

        public string EmployerAccountId { get; }
    }
}