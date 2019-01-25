using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class UnSubmitReportHandler : RequestHandler<UnSubmitReportRequest>
    {
        private IMapper _mapper;
        private IReportRepository _reportRepository;

        public UnSubmitReportHandler(IMapper mapper, IReportRepository reportRepository)
        {
            _mapper = mapper;
            _reportRepository = reportRepository;
        }
        protected override void HandleCore(UnSubmitReportRequest request)
        {
            var reportDto = _mapper.Map<ReportDto>(request.Report);

            if (reportDto == null)
                return;

            reportDto.Submitted = false;

            _reportRepository.Update(reportDto);
        }
    }
}