using System;
using AutoMapper;
using MediatR;
using SFA.DAS.NServiceBus;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Messages.Events;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class SubmitReportHandler : RequestHandler<SubmitReportRequest>
    {
        private IMapper _mapper;
        private IReportRepository _reportRepository;
        private readonly IEventPublisher _eventPublisher;

        public SubmitReportHandler(
            IMapper mapper, 
            IReportRepository reportRepository, 
            IEventPublisher eventPublisher)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }
        protected override void HandleCore(SubmitReportRequest request)
        {
            var reportDto = _mapper.Map<ReportDto>(request.Report);

            if (reportDto == null)
                return;

            reportDto.Submitted = true;

            _reportRepository.Update(reportDto);

            _reportRepository.DeleteHistory(reportDto.Id);

            _eventPublisher.Publish(new ReportSubmitted());
        }
    }
}