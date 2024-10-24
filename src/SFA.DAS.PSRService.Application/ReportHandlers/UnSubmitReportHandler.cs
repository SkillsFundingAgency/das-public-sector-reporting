using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.Application.ReportHandlers;

public class UnSubmitReportHandler(IReportRepository reportRepository) : IRequestHandler<UnSubmitReportRequest>
{
    public async Task Handle(UnSubmitReportRequest request, CancellationToken cancellationToken)
    {
        var report = await reportRepository.Get(request.ReportingPeriod.PeriodString, request.HashedEmployerAccountId);

        if (report == null)
        {
            return;
        }

        report.Submitted = false;

        await reportRepository.Update(report);
    }
}