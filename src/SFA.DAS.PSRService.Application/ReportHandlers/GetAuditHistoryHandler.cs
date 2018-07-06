using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class GetReportEditHistoryMostRecentFirstHandler : RequestHandler<GetReportEditHistoryMostRecentFirst, IEnumerable<AuditRecord>>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public GetReportEditHistoryMostRecentFirstHandler(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        protected override IEnumerable<AuditRecord> HandleCore(GetReportEditHistoryMostRecentFirst mostRecentFirst)
        {
            var report =
                _reportRepository
                    .Get(
                        period: mostRecentFirst.Period.PeriodString,
                        employerId: mostRecentFirst.AccountId);

            if(report == null)
                return
                    Enumerable.Empty<AuditRecord>();

            if (report.Submitted)
                return
                    Enumerable.Empty<AuditRecord>();

            return
                _reportRepository
                    .GetAuditRecordsMostRecentFirst(
                        report.Id)
                    .Select(
                        dto => _mapper.Map<AuditRecord>(dto));
        }
    }
}