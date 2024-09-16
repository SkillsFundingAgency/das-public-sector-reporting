using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.Application.ReportHandlers;

public class SubmitReportHandler(IMapper mapper, IReportRepository reportRepository) : IRequestHandler<SubmitReportRequest>
{
    public Task Handle(SubmitReportRequest request, CancellationToken cancellationToken)
    {
        var reportDto = mapper.Map<ReportDto>(request.Report);

        if (reportDto == null)
        {
            return Task.CompletedTask;
        }

        reportDto.Submitted = true;

        reportRepository.Update(reportDto);

        reportRepository.DeleteHistory(reportDto.Id);
        
        return Task.CompletedTask;
    }
}