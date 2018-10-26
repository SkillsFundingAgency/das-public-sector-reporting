using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.FileProviders;
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
        private readonly IFileProvider _fileProvider;
        private readonly IEventPublisher _eventPublisher;

        public CreateReportHandler(
            IReportRepository reportRepository, 
            IMapper mapper, 
            IFileProvider fileProvider, 
            IEventPublisher eventPublisher)
        {
            _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }

        protected override Report HandleCore(CreateReportRequest request)
        {
            var reportDto = new ReportDto
            {
                EmployerId = request.EmployerId,
                Submitted = false,
                Id = Guid.NewGuid(),
                ReportingPeriod = request.Period,
                ReportingData = GetQuestionConfig().Result,
                AuditWindowStartUtc = DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow,
                UpdatedBy = JsonConvert.SerializeObject(new User {Id = request.User.Id, Name = request.User.Name})
            };

            _reportRepository.Create(reportDto);

            var createdReport = _mapper.Map<Report>(reportDto);

            _eventPublisher
                .Publish(
                    _mapper.Map<ReportCreated>(createdReport));

            return createdReport;
        }

        private Task<string> GetQuestionConfig()
        {
            var questionsConfig = _fileProvider.GetFileInfo("/QuestionConfig.json");

            using (var jsonContents = questionsConfig.CreateReadStream())
            using (StreamReader sr = new StreamReader(jsonContents))
            {
                return Task.FromResult(sr.ReadToEnd());
            }
        }
    }
}