using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers;

public class GetReportEditHistoryMostRecentFirstHandler : IRequestHandler<GetReportEditHistoryMostRecentFirst, IEnumerable<AuditRecord>>
{
    private readonly IReportRepository _reportRepository;
    private readonly IMapper _mapper;

    public GetReportEditHistoryMostRecentFirstHandler(IReportRepository reportRepository, IMapper mapper)
    {
        _reportRepository = reportRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AuditRecord>> Handle(GetReportEditHistoryMostRecentFirst request, CancellationToken cancellationToken)
    {
        var report = _reportRepository.Get(period: request.Period.PeriodString, employerId: request.AccountId);

        if (report == null)
        {
            return [];
        }

        if (report.Submitted)
        {
            return [];
        }

        return _reportRepository.GetAuditRecordsMostRecentFirst(report.Id).Select(dto => _mapper.Map<AuditRecord>(dto));
    }
}