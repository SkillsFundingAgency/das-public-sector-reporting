using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers;

public class GetReportEditHistoryMostRecentFirstHandler(IReportRepository reportRepository, IMapper mapper) : IRequestHandler<GetReportEditHistoryMostRecentFirst, IEnumerable<AuditRecord>>
{
    public async Task<IEnumerable<AuditRecord>> Handle(GetReportEditHistoryMostRecentFirst request, CancellationToken cancellationToken)
    {
        var report = await reportRepository.Get(period: request.Period.PeriodString, employerId: request.AccountId);

        if (report == null)
        {
            return [];
        }

        if (report.Submitted)
        {
            return [];
        }

        var auditRecordDtos = await reportRepository.GetAuditRecordsMostRecentFirst(report.Id);

        return auditRecordDtos.Select(mapper.Map<AuditRecord>);
    }
}