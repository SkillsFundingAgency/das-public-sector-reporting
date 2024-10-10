using System.Collections.Generic;
using MediatR;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers;

public class GetSubmittedRequest : IRequest<IEnumerable<Report>>
{
    public string EmployerId { get; set; }
}