using System;
using MediatR;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class CreateReportRequest : IRequest<Report>
    {
        public CreateReportRequest(
            User user, 
            string employerId, 
            string period,
            bool isLocalAuthority)
        {
            if (string.IsNullOrWhiteSpace(employerId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(employerId));

            if (string.IsNullOrWhiteSpace(period))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(period));

            EmployerId = employerId;
            Period = period;
            IsLocalAuthority = isLocalAuthority;

            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        public string EmployerId { get; }
        public string Period { get; }
        public bool IsLocalAuthority { get; }      
        public User User { get; }
    }
}