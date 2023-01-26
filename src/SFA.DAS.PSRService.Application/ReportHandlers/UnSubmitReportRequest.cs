using System;
using MediatR;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class UnSubmitReportRequest : IRequest
    {
        public UnSubmitReportRequest(string hashedEmployerAccountId, Period reportingPeriod)
        {
            if (string.IsNullOrWhiteSpace(hashedEmployerAccountId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(hashedEmployerAccountId));

            HashedEmployerAccountId = hashedEmployerAccountId;
            ReportingPeriod = reportingPeriod ?? throw new ArgumentNullException(nameof(reportingPeriod));
        }

        public Period ReportingPeriod { get; }

        public string HashedEmployerAccountId { get; }
    }
}