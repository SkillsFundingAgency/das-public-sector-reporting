using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class GetReportHandler : IRequestHandler<GetReportRequest, Report>
    {
        private readonly IReportRepository _reportRepository;

        public GetReportHandler(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public Task<Report> Handle(GetReportRequest request, CancellationToken cancellationToken)
        {
            var json = _reportRepository.Get(request.ReportId);
            var report = new Report {Data = json};
            return Task.FromResult(report);
        }
    }
}