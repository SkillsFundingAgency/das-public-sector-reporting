using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers;

public class GetSubmittedHandler(IReportRepository reportRepository, IMapper mapper) : IRequestHandler<GetSubmittedRequest, IEnumerable<Report>>
{
    public Task<IEnumerable<Report>> Handle(GetSubmittedRequest request, CancellationToken cancellationToken)
    {
        return string.IsNullOrEmpty(request.EmployerId)
            ? Task.FromResult<IEnumerable<Report>>([])
            : Task.FromResult(reportRepository.GetSubmitted(request.EmployerId).Select(mapper.Map<Report>));
    }
}