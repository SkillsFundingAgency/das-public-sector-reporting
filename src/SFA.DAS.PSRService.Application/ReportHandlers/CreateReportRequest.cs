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
            Period period)
        {
            if (string.IsNullOrWhiteSpace(employerId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(employerId));

            EmployerId = employerId;
            
            Period = period ?? throw new ArgumentNullException(nameof(period));

            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        public string EmployerId { get; }
        public Period Period { get; }

        public User User { get; }
    }
}