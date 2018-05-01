using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class UpdateReportHandler : IRequestHandler<UpdateReportRequest>
    {
        private IMapper _mapper;
        private IReportRepository _reportRepository;

        public UpdateReportHandler(IMapper mapper, IReportRepository reportRepository)
        {
            _mapper = mapper;
            _reportRepository = reportRepository;
        }
        public Task Handle(UpdateReportRequest request, CancellationToken cancellationToken)
        {
            if (request.Report == null)
            {
                throw new Exception("No Report Supplied");
            }

            request.Report.UpdatePercentages();

            var reportDto = _mapper.Map<ReportDto>(request.Report);

            _reportRepository.Update(reportDto);

          return  Task.FromResult(0);
        }
    }
}