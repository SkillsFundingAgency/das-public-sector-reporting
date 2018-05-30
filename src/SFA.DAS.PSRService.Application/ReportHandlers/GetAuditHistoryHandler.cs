using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class GetAuditHistoryHandler : RequestHandler<GetAuditHistoryRequest, IEnumerable<AuditRecord>>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public GetAuditHistoryHandler(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        protected Report HandleCore(GetReportRequest request)
        {
            var reportDto = _reportRepository.Get(request.Period,request.EmployerId);

            return _mapper.Map<Report>(reportDto);
        }

        protected override IEnumerable<AuditRecord> HandleCore(GetAuditHistoryRequest request)
        {
            var report =
                _reportRepository
                    .Get(
                        period: request.Period.PeriodString,
                        employerId: request.AccountId);

            if(report == null)
                return
                    Enumerable.Empty<AuditRecord>();

            if (report.Submitted)
                return
                    Enumerable.Empty<AuditRecord>();

            return
                _reportRepository
                    .GetAuditRecords(
                        report.Id)
                    .Select(
                        dto => _mapper.Map<AuditRecord>(dto));
        }
    }
}