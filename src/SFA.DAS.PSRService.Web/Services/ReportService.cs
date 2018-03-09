using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;

namespace SFA.DAS.PSRService.Web.Services
{
    public class ReportService : IReportService
    {
        private readonly IWebConfiguration _config;
        private IMediator _mediator;

        public ReportService(IWebConfiguration config, IMediator mediator)
        {
            _config = config;
            _mediator = mediator;
        }

        public Report GetReport(Guid reportId)
        {
            var request = new GetReportRequest() {ReportId = reportId};
            var report = _mediator.Send(request).Result;
            return report;
        }
    }
}
