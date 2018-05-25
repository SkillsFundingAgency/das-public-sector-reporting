using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class GetAuditHistoryHandler : RequestHandler<GetAuditHistoryRequest, AuditRecord>
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

        protected override AuditRecord HandleCore(GetAuditHistoryRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}