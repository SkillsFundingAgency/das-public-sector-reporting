using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class SubmitReportHandler : IRequestHandler<SubmitReportRequest>
    {
        private IMapper _mapper;
        private IReportRepository _reportRepository;

        public SubmitReportHandler(IMapper mapper, IReportRepository reportRepository)
        {
            _mapper = mapper;
            _reportRepository = reportRepository;
        }
        public Task Handle(SubmitReportRequest request, CancellationToken cancellationToken)
        {
            if (request.Report == null)
            {
                throw new Exception("No Report Supplied");
            }
            var reportDto = _mapper.Map<ReportDto>(request.Report);

            reportDto.Submitted = true;
            _reportRepository.Update(reportDto);

          return  Task.FromResult(0);
        }
    }
}