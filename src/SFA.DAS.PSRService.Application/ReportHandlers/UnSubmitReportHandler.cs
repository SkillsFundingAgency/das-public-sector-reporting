using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.Application.ReportHandlers;

public class UnSubmitReportHandler(IReportRepository reportRepository) : IRequestHandler<UnSubmitReportRequest>
{
    public Task Handle(UnSubmitReportRequest request, CancellationToken cancellationToken)
    {
        var report = reportRepository.Get(request.ReportingPeriod.PeriodString, request.HashedEmployerAccountId);

        if (report == null)
        {
            return Task.CompletedTask;
        }

        report.Submitted = false;

        reportRepository.Update(report);

        return Task.CompletedTask;
    }
}