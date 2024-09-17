using System;
using MediatR;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers;

public class CreateReportRequest : IRequest<Report>
{
    public string EmployerId { get; }
    public string Period { get; }
    public bool IsLocalAuthority { get; }      
    public User User { get; }
    
    public CreateReportRequest(User user, string employerId, string period, bool? isLocalAuthority)
    {
        if (string.IsNullOrWhiteSpace(employerId))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(employerId));

        if (string.IsNullOrWhiteSpace(period))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(period));

        if (!isLocalAuthority.HasValue)
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(isLocalAuthority));

        EmployerId = employerId;
        Period = period;
        IsLocalAuthority = isLocalAuthority.Value;
        User = user ?? throw new ArgumentNullException(nameof(user));
    }
}