using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MediatR;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class GetSubmittedHandler : RequestHandler<GetSubmittedRequest, IEnumerable<Report>>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public GetSubmittedHandler(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        protected override IEnumerable<Report> HandleCore(GetSubmittedRequest request)
        {
            if (string.IsNullOrEmpty(request.EmployerId))
                return Enumerable.Empty<Report>();

            return
                _reportRepository
                    .GetSubmitted(request.EmployerId)
                    .Select(data => _mapper.Map<Report>(data));
        }
    }
}