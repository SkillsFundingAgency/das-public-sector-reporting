using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class GetSubmittedHandler : IRequestHandler<GetSubmittedRequest, IEnumerable<Report>>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public GetSubmittedHandler(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        public Task<IEnumerable<Report>> Handle(GetSubmittedRequest request, CancellationToken cancellationToken)
        {
            var reportDtoList = _reportRepository.GetSubmitted(request.EmployerId);

            var reportList = reportDtoList.Select(data => _mapper.Map<Report>(data));

            return Task.FromResult(reportList);
        }
    }
}