using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class CreateReportHandler : IRequestHandler<CreateReportRequest,Report>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public CreateReportHandler(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        public Task<Report> Handle(CreateReportRequest request, CancellationToken cancellationToken)
        {
          var reportDto = _reportRepository.Create(request.EmployerId, request.Period);

            var report = _mapper.Map<Report>(reportDto);

            return Task.FromResult(report);
        }
    }
}