using System;
using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class SubmitReportHandler : RequestHandler<SubmitReportRequest>
    {
        private IMapper _mapper;
        private IReportRepository _reportRepository;

        public SubmitReportHandler(IMapper mapper, IReportRepository reportRepository)
        {
            _mapper = mapper;
            _reportRepository = reportRepository;
        }
        protected override void HandleCore(SubmitReportRequest request)
        {
            var reportDto = _mapper.Map<ReportDto>(request.Report);

            reportDto.Submitted = true;

            _reportRepository.Update(reportDto);
        }
    }
}