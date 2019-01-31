using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class UnSubmitReportHandler : RequestHandler<UnSubmitReportRequest>
    {
        private IReportRepository _reportRepository;

        public UnSubmitReportHandler(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        protected override void HandleCore(UnSubmitReportRequest request)
        {
            var report = _reportRepository.Get(request.ReportingPeriod.PeriodString, request.EmployerAccountId);

            if (report == null)
                return;

            report.Submitted = false;

            _reportRepository.Update(report);
        }
    }
}