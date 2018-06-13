using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class UpdateReportHandler : RequestHandler<UpdateReportRequest>
    {
        private IMapper _mapper;
        private IReportRepository _reportRepository;

        public UpdateReportHandler(IMapper mapper, IReportRepository reportRepository)
        {
            _mapper = mapper;
            _reportRepository = reportRepository;
        }
        protected override void HandleCore(UpdateReportRequest request)
        {
            request
                .Report
                .UpdatePercentages();

            var reportDto = _mapper.Map<ReportDto>(request.Report);

            _reportRepository.Update(reportDto);
        }
    }
}