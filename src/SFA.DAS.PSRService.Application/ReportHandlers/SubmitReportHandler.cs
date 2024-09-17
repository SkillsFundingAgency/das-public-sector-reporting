using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.Application.ReportHandlers;

public class SubmitReportHandler(IMapper mapper, IReportRepository reportRepository) : IRequestHandler<SubmitReportRequest>
{
    public async Task Handle(SubmitReportRequest request, CancellationToken cancellationToken)
    {
        var reportDto = mapper.Map<ReportDto>(request.Report);

        if (reportDto == null)
        {
            return;
        }

        reportDto.Submitted = true;

        await reportRepository.Update(reportDto);

        await reportRepository.DeleteHistory(reportDto.Id);
    }
}