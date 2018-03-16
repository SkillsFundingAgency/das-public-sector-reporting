using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class GetSubmittedRequest : IRequest<IEnumerable<Report>>
    {
        public long EmployerId { get; set; }
    }

    
    }

