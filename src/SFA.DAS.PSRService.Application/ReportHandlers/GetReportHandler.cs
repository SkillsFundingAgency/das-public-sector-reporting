using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class GetReportHandler : RequestHandler<GetReportRequest, Report>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public GetReportHandler(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        protected override Report HandleCore(GetReportRequest request)
        {
            var reportDto = _reportRepository.Get(request.Period.PeriodString,request.EmployerId);

            return _mapper.Map<Report>(reportDto);
        }
    }
}