using System;
using MediatR;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class CreateReportRequest : IRequest<Report>
    {
        public string EmployerId { get; set; }
        public string Period { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}