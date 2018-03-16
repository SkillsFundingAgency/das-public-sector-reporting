using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class GetReportHandler : IRequestHandler<GetReportRequest, Report>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public GetReportHandler(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        public Task<Report> Handle(GetReportRequest request, CancellationToken cancellationToken)
        {
            var reportDto = _reportRepository.Get(request.Period,request.EmployerId);

            var report = _mapper.Map<Report>(reportDto);

            return Task.FromResult(report);
        }
    }
}