using System;
using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.NServiceBus;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Messages.Events;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class CreateReportHandler : RequestHandler<CreateReportRequest, Report>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        private readonly QuestionConfigProvider _questionConfigProvider;
        private readonly IEventPublisher _eventPublisher;

        public CreateReportHandler(
            IReportRepository reportRepository, 
            IMapper mapper, 
            QuestionConfigProvider questionConfigSource, 
            IEventPublisher eventPublisher)
        {
            _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _questionConfigProvider = questionConfigSource ?? throw new ArgumentNullException(nameof(questionConfigSource));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }

        protected override Report HandleCore(CreateReportRequest request)
        {
            var reportDto = new ReportDto
            {
                EmployerId = request.EmployerId,
                Submitted = false,
                Id = Guid.NewGuid(),
                ReportingPeriod = request.Period.PeriodString,
                ReportingData = _questionConfigProvider.GetNewlyCreatedReportQuestionConfig(),
                AuditWindowStartUtc = DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow,
                UpdatedBy = JsonConvert.SerializeObject(new User {Id = request.User.Id, Name = request.User.Name})
            };

            _reportRepository.Create(reportDto);

            var createdReport = _mapper.Map<Report>(reportDto);

            var message = _mapper.Map<ReportCreated>(createdReport);

            _eventPublisher
                .Publish(
                    message);

            return createdReport;
        }

    }
}