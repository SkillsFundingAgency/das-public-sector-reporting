using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{

    public class GetReportRequest : IRequest<Report>
    {
        public string EmployerId { get; set; }
        public Period Period { get; set; }
    }
    }

