using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class CreateReportRequest : IRequest<Report>
    {
    }

    public class GetReportRequest : IRequest<Report>
    {
        public Guid ReportId { get; set; }
        public long EmployerId { get; set; }
        public string Period { get; set; }
    }
    }

