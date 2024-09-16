using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers;

public class GetReportHandler(IReportRepository reportRepository, IMapper mapper) : IRequestHandler<GetReportRequest, Report>
{
    public async Task<Report> Handle(GetReportRequest request, CancellationToken cancellationToken)
    {
        var reportDto = reportRepository.Get(request.Period,request.EmployerId);

        return await Task.FromResult(mapper.Map<Report>(reportDto));
    }
}