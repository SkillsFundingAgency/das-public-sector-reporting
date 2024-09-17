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
    public Task<IEnumerable<AuditRecord>> Handle(GetReportEditHistoryMostRecentFirst request, CancellationToken cancellationToken)
    {
        var report = reportRepository.Get(period: request.Period.PeriodString, employerId: request.AccountId);

        if (report == null)
        {
            return Task.FromResult<IEnumerable<AuditRecord>>([]);
        }

        return report.Submitted 
            ? Task.FromResult<IEnumerable<AuditRecord>>([]) 
            : Task.FromResult(reportRepository.GetAuditRecordsMostRecentFirst(report.Id).Select(dto => mapper.Map<AuditRecord>(dto)));
    }
}